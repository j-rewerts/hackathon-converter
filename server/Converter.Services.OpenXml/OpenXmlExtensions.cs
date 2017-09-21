using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Converter.Services.OpenXml
{
    public static class OpenXmlExtensions
    {
        internal static string TryGetStringFromCell(this WorkbookPart workbookPart, Cell cell)
        {
            if (cell.DataType == null || !cell.DataType.HasValue)
            {
                if (cell.CellValue == null)
                    return null;
                else
                    return cell.CellValue.Text;
            }

            if (cell.DataType.Value == CellValues.String)
                return cell.CellValue.Text;
            else if (cell.DataType.Value == CellValues.InlineString)
                return cell.InnerText;
            else if (cell.DataType.Value == CellValues.SharedString)
            {
                // following based on https://msdn.microsoft.com/en-us/library/office/hh298534.aspx
                // For shared strings, look up the value in the
                // shared strings table.
                var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                if (stringTable == null)
                {
                    Exception err = new InvalidOperationException(message: "Unable to get shared string table from Excel document");
                    throw err;
                }
                else
                {
                    return stringTable.SharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
                }
            }

            return null;
        }

        internal static decimal? TryGetNumberFromCell(this WorkbookPart workbookPart, Cell cell)
        {
            // probably not the right way to do this...
            string s = TryGetStringFromCell(workbookPart, cell);
            if (!string.IsNullOrWhiteSpace(s))
            {
                decimal d;
                if (decimal.TryParse(s, out d))
                    return d;
            }
            return null;
        }

        internal static bool IsStrikethrough(this WorkbookPart workbookPart, Cell cell)
        {
            // based on https://social.msdn.microsoft.com/Forums/sqlserver/en-US/f8a9f23b-6b65-46c0-b584-f075bb1332ea/cell-strikethough-check-for?forum=oxmlsdk

            if (cell == null)
                throw new ArgumentNullException("cell");

            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        var value = cell.InnerText;
                        var stringTable = workbookPart
                            .GetPartsOfType<SharedStringTablePart>()
                            .FirstOrDefault();

                        if (stringTable != null)
                        {
                            //detect whther the text was part of striked from string item
                            foreach (Strike strike in stringTable.SharedStringTable
                               .ElementAt(int.Parse(value)).Descendants<Strike>())
                            {
                                if (strike.Val == null || strike.Val != false)
                                {
                                    return true;
                                }
                            }
                        }

                        //detect whther the text was striked from cell style
                        var cellFormat = (CellFormat) workbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt(int.Parse(cell.StyleIndex));
                        var font = workbookPart.WorkbookStylesPart.Stylesheet.Fonts.ElementAt(int.Parse(cellFormat.FontId));
                        foreach (Strike strike in font.Descendants<Strike>())
                        {
                            if (strike.Val == null || strike.Val != false)
                            {
                                return true;
                            }
                        }
                        break;
                }
            }
            return false;
        }

        internal readonly static Regex CellReferenceMatcher = new Regex(@"^\$?([a-zA-Z]+)\$?(\d+)$");
        internal readonly static Regex ColumnReferenceMatcher = new Regex(@"^\$?([a-zA-Z]+)\$?(\d*)$");

        internal static int GetRowIndex(this Cell cell)
        {
            var cellRef = cell.CellReference;
            if (!cellRef.HasValue)
                throw new InvalidOperationException("Unable to get cellReference value (string value is missing)");
            string s = cellRef.Value;
            if (string.IsNullOrWhiteSpace(s))
                throw new InvalidOperationException("Unable to get a cellReference value (string value is empty)");

            var match = CellReferenceMatcher.Match(s);
            // note that in .NET the first group is the full match
            if (match.Success && match.Groups.Count >= 3 && match.Groups[2].Success && !string.IsNullOrWhiteSpace(match.Groups[2].Value))
            {
                var rowIndexGroup = match.Groups[2];
                int index;
                if (int.TryParse(rowIndexGroup.Value, out index))
                    return index;
                else
                    throw new InvalidOperationException($"Expected integer value for cellReference { s }, got { rowIndexGroup.Value }");
            }
            else
                throw new InvalidOperationException($"Unable to get a row index from cellReference { s }");
        }

        internal static string GetColumnReference(this Cell cell)
        {
            var cellRef = cell.CellReference;
            if (!cellRef.HasValue)
                throw new InvalidOperationException("Unable to get cellReference value (string value is missing)");
            return GetExcelColumnReference(cellRef.Value);
        }

        internal static string GetExcelColumnReference(this string cellReference)
        {
            string s = cellReference;
            if (string.IsNullOrWhiteSpace(s))
                throw new InvalidOperationException("Unable to get a cellReference value (string value is empty)");

            var match = ColumnReferenceMatcher.Match(s);
            // note that in .NET the first group is the full match
            if (match.Success && match.Groups.Count >= 3 && match.Groups[1].Success && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
            {
                return match.Groups[1].Value;
            }
            else
                throw new InvalidOperationException($"Unable to get a column index from cellReference { s }");
        }

        internal static int GetColumnReferenceIndex(this Cell cell)
        {
            var columnRef = GetColumnReference(cell).ToUpperInvariant().ToCharArray();
            //A-Z returns 0-25,
            //AA-AZ returns 26 - 51
            //BA - BZ returns 52 - 78
            //AAA returns 675 - 702
            // etc.
            int returnValue = 0, len = columnRef.Length-1;
            for (int i=0; i<=len; i++)
            {
                char c = columnRef[i];
                //Note A=65, Z=90
                if (i < len)
                {
                    int offset = (int) Math.Pow(26, len - i);
                    offset = offset * (c - 64);
                    returnValue += offset;
                }
                else
                    returnValue += (c - 65);
            }
            return returnValue +1;
        }

        internal static int GetExcelColumnReferenceIndex(this string cellReference)
        {
            var columnRef = GetExcelColumnReference(cellReference).ToCharArray();
            //A-Z returns 0-25,
            //AA-AZ returns 26 - 51
            //BA - BZ returns 52 - 78
            //AAA returns 675 - 702
            // etc.
            int returnValue = 0, len = columnRef.Length - 1;
            for (int i = 0; i <= len; i++)
            {
                char c = columnRef[i];
                //Note A=65, Z=90
                if (i < len)
                {
                    int offset = (int) Math.Pow(26, len - i);
                    offset = offset * (c - 64);
                    returnValue += offset;
                }
                else
                    returnValue += (c - 65);
            }
            return returnValue;
        }


        //internal static void GetCellStringValues(this WorkbookPart workbookPart, 
        //    IEnumerable<Cell> cells, Action<string> values)
        //{
        //    foreach (var cell in cells)
        //    {
        //        values(TryGetStringFromCell(workbookPart, cell));
        //    }
        //}

        //internal static void GetCellStringValues(this WorkbookPart workbookPart,
        //    IEnumerable<Cell> cells, Action<string, Cell> values)
        //{
        //    foreach (var cell in cells)
        //    {
        //        values(TryGetStringFromCell(workbookPart, cell), cell);
        //    }
        //}

        // based on http://ericwhite.com/blog/handling-invalid-hyperlinks-openxmlpackageexception-in-the-open-xml-sdk/
        public static void FixInvalidUri(System.IO.Stream fs, Func<string, Uri> invalidUriHandler)
        {
            System.Xml.Linq.XNamespace relNs = "http://schemas.openxmlformats.org/package/2006/relationships";
            using (var za = new System.IO.Compression.ZipArchive(fs, System.IO.Compression.ZipArchiveMode.Update))
            {
                foreach (var entry in za.Entries.ToList())
                {
                    if (!entry.Name.EndsWith(".rels"))
                        continue;
                    bool replaceEntry = false;
                    System.Xml.Linq.XDocument entryXDoc = null;
                    using (var entryStream = entry.Open())
                    {
                        try
                        {
                            entryXDoc = System.Xml.Linq.XDocument.Load(entryStream);
                            if (entryXDoc.Root != null && entryXDoc.Root.Name.Namespace == relNs)
                            {
                                var urisToCheck = entryXDoc
                                    .Descendants(relNs + "Relationship")
                                    .Where(r => r.Attribute("TargetMode") != null && (string) r.Attribute("TargetMode") == "External");
                                foreach (var rel in urisToCheck)
                                {
                                    var target = (string) rel.Attribute("Target");
                                    if (target != null)
                                    {
                                        try
                                        {
                                            Uri uri = new Uri(target);
                                        }
                                        catch (UriFormatException)
                                        {
                                            Uri newUri = invalidUriHandler(target);
                                            rel.Attribute("Target").Value = newUri.ToString();
                                            replaceEntry = true;
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Xml.XmlException)
                        {
                            continue;
                        }
                    }
                    if (replaceEntry)
                    {
                        var fullName = entry.FullName;
                        entry.Delete();
                        var newEntry = za.CreateEntry(fullName);
                        using (var writer = new System.IO.StreamWriter(newEntry.Open()))
                        using (var xmlWriter = System.Xml.XmlWriter.Create(writer))
                        {
                            entryXDoc.WriteTo(xmlWriter);
                        }
                    }
                }
            }
        }
    }
}

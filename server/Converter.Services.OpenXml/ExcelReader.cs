using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace Converter.Services.OpenXml
{
    public class ExcelReader
    {
        private Stream file;
        private Workbook workbook;
        private IList<Worksheet> worksheets;
        private Stream vbaStream;

        public ExcelReader(Stream file, string GoogleFileID)
        {
            this.file = file;
            this.workbook = new Workbook();
            this.workbook.GoogleFileID = GoogleFileID;
            this.workbook.HasCustomCode = false;
            this.workbook.HasDataConnections = false;
            this.workbook.HasExternalConnections = false;
            this.workbook.HasExternalHyperLinks = false;
            this.workbook.HasExternalRelationships = false;
            this.worksheets = new List<Worksheet>();
        }
        public Workbook GetWorkbook()
        {
            return this.workbook;
        }
        public IEnumerable<Worksheet> GetWorksheets()
        {
            return this.worksheets;
        }
        public IEnumerable<CellInfo> ReadFile()//Action<object> onCellValueRead)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(this.file, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                DoAdditionalChecks(workbookPart);
                foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
                {
                    var sheetProperties = worksheetPart.RootElement.FirstChild as SheetProperties;
                    string sheetName = sheetProperties.CodeName.Value;
                    foreach (SheetData sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                    {

                        foreach (var cellInfo in GetCellValues(workbookPart, sheetData, sheetName))
                            yield return cellInfo;
                    }
                }
            }
        }
        private void DoAdditionalChecks(WorkbookPart workbookPart)
        {
            DetermineExternalConnections(workbookPart);
            DetermineCustomCode(workbookPart);
            DetermineConnections(workbookPart);
            DetermineHyperLinks(workbookPart);
            DetermineExternalRelationships(workbookPart);
            //ConnectionsPart connectionsPart = workbookPart.ConnectionsPart;
        }
        private void DetermineExternalRelationships(WorkbookPart workbookPart)
        {
            foreach (ExternalRelationship externalRelationship in workbookPart.ExternalRelationships)
            {
                this.workbook.HasExternalRelationships = true;
            }
        }
        private void DetermineHyperLinks(WorkbookPart workbookPart)
        {
            foreach (HyperlinkRelationship hyperlinkRelationship in workbookPart.HyperlinkRelationships)
            {
                this.workbook.HasExternalHyperLinks = true;
            }
        }
        private void DetermineCustomCode(WorkbookPart workbookPart)
        {
            if (workbookPart.VbaProjectPart != null)
            {
                VbaProjectPart vbaProjectPart = workbookPart.VbaProjectPart;
                this.workbook.HasCustomCode = true;
            }
        }
        private void DetermineExternalConnections(WorkbookPart workbookPart)
        {
            foreach (ExternalWorkbookPart externalWorkbookPart in workbookPart.ExternalWorkbookParts)
            {
                this.workbook.HasExternalConnections = true;
                string relationship = externalWorkbookPart.RelationshipType;
            }
        }
        private void DetermineConnections(WorkbookPart workbookPart)
        {
            if (workbookPart.ConnectionsPart != null)
            {
                this.workbook.HasDataConnections = true;
            }
        }

        private IEnumerable<CellInfo> GetCellValues(WorkbookPart workbookPart, SheetData sheetData, string sheetName)
        {
            string text = "";
            var worksheetInfo = new Worksheet();
            foreach (Row r in sheetData.Elements<Row>())
            {
                if (worksheetInfo.FirstRow == 0)
                {
                    worksheetInfo.FirstRow = r.RowIndex.Value;
                    Cell cellFirst = (Cell)r.FirstChild;
                    worksheetInfo.FirstColumn = (uint)cellFirst.GetColumnReferenceIndex();
                    Cell cellLast = (Cell)r.LastChild;
                    worksheetInfo.LastColumn = (uint)cellLast.GetColumnReferenceIndex();
                }
                else
                {
                    Cell cellFirst = (Cell)r.FirstChild;
                    uint firstColumn = (uint)cellFirst.GetColumnReferenceIndex();
                    Cell cellLast = (Cell)r.LastChild;
                    uint lastColumn = (uint)cellLast.GetColumnReferenceIndex();
                    if (firstColumn < worksheetInfo.FirstColumn)
                    {
                        worksheetInfo.FirstColumn = firstColumn;
                    }
                    if (lastColumn > worksheetInfo.LastColumn)
                    {
                        worksheetInfo.LastColumn = lastColumn;
                    }
                }
                worksheetInfo.LastRow = r.RowIndex.Value;

                foreach (Cell c in r.Elements<Cell>())
                {
                    //c.GetRowIndex();
                    text = workbookPart.TryGetStringFromCell(c);// c.CellValue.Text;
                    Console.Write(text + " ");
                    yield return new CellInfo() { Cell = c, Value = text, SheetName = sheetName };
                }
            }
            this.worksheets.Add(worksheetInfo);
        } // end get cell values

        private void GetVbaStream(VbaProjectPart vbaProjectPart)
        {
            if (vbaProjectPart == null)
            {
                this.vbaStream = null;
            }
            else
            {
                this.vbaStream = vbaProjectPart.GetStream();
            }
        }
    }
}

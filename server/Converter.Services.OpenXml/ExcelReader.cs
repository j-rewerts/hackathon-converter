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
    public class ExcelReader : IDisposable
    {
        private Stream file;
        private Workbook workbook;
        private IList<Worksheet> worksheets;
        private Stream vbaStream;

        public ExcelReader(Stream file, string GoogleFileID)
        {
            this.file = file;
            workbook = new Workbook();
            workbook.GoogleFileID = GoogleFileID;
            workbook.HasCustomCode = false;
            workbook.HasDataConnections = false;
            workbook.HasExternalConnections = false;
            workbook.HasExternalHyperLinks = false;
            workbook.HasExternalRelationships = false;
            worksheets = new List<Worksheet>();

            try
            {
                spreadsheetDocument = SpreadsheetDocument.Open(this.file, false);
                workbookPart = spreadsheetDocument.WorkbookPart;
            }
            catch (Exception ex)
            {
                throw new NotSpreadSheetException("Can't read file", ex);
            }


        }

        private SpreadsheetDocument spreadsheetDocument;
        private WorkbookPart workbookPart;

        public Workbook GetWorkbook()
        {
            return this.workbook;
        }
        public IEnumerable<string> GetSheetNames()
        {
            int sheetIndex = 0;
            foreach (WorksheetPart worksheetpart in workbookPart.WorksheetParts)
            {
                var worksheet = worksheetpart.Worksheet;

                // Grab the sheet name each time through your loop
                string sheetName = workbookPart.Workbook.Descendants<Sheet>().ElementAt(sheetIndex).Name;

                yield return sheetName;
                sheetIndex++;
            }
        }

        public IEnumerable<Worksheet> GetWorksheets()
        {
            return this.worksheets;
        }
        public IEnumerable<CellInfo> ReadFile()//Action<object> onCellValueRead)
        {
            DoAdditionalChecks(workbookPart);
            int sheetIndex = 0;
            foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
            {
                var sheetProperties = worksheetPart.RootElement.FirstChild as SheetProperties;
                //string sheetName = sheetProperties.CodeName.Value;

                var worksheet = worksheetPart.Worksheet;
                // Grab the sheet name each time through your loop
                string sheetName = workbookPart.Workbook.Descendants<Sheet>().ElementAt(sheetIndex).Name;

                foreach (SheetData sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                {
                    foreach (var cellInfo in GetCellValues(workbookPart, sheetData, sheetName))
                        yield return cellInfo;
                }

                sheetIndex++;
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

        private IEnumerable<CellInfo> GetCellValues(WorkbookPart workbookPart, 
            SheetData sheetData, string sheetName)
        {
            string text = "";
            string formula = "";
            var worksheetInfo = new Worksheet();
            worksheetInfo.Name = sheetName;
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
                    formula = c.CellFormula?.InnerText;
                    Console.Write(text + " ");
                    yield return new CellInfo() { Cell = c, Value = text, SheetName = sheetName, Formula = formula };
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (spreadsheetDocument != null)
                    {
                        spreadsheetDocument.Dispose();
                        spreadsheetDocument = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ExcelReader() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

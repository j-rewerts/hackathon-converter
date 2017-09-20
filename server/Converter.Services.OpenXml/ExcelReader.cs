using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Converter.Services.OpenXml
{
    public class ExcelReader
    {
        private Stream file;
        private Workbook workbook;
        private IList<Worksheet> worksheets;
        
        public ExcelReader(Stream file, string GoogleFileID)
        {
            this.file = file;
            this.workbook = new Workbook();
            this.workbook.GoogleFileID = GoogleFileID;
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
                    var worksheetInfo = new Worksheet();
                    this.worksheets.Add(worksheetInfo);
                    foreach (SheetData sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                        foreach (var cellInfo in GetCellValues(workbookPart, sheetData))
                            yield return cellInfo;
                    
                }
                //WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                //SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            }
        }
        private void DoAdditionalChecks(WorkbookPart workbookPart)
        {
            DetermineExternalConnections(workbookPart);
            ConnectionsPart connectionsPart = workbookPart.ConnectionsPart;
        }
        private void DetermineExternalConnections(WorkbookPart workbookPart)
        {
            foreach (ExternalWorkbookPart externalWorkbookPart in workbookPart.ExternalWorkbookParts)
            {
                this.workbook.HasExternalConnections = true;
                string relationship = externalWorkbookPart.RelationshipType;
            }
        }
        private IEnumerable<CellInfo> GetCellValues(WorkbookPart workbookPart, SheetData sheetData)
        {
            string text = "";
            string sheetName = "";
            foreach (Row r in sheetData.Elements<Row>())
            {
                foreach (Cell c in r.Elements<Cell>())
                {
                    c.GetRowIndex();
                    text = workbookPart.TryGetStringFromCell(c);// c.CellValue.Text;
                    Console.Write(text + " ");
                    yield return new CellInfo() { Cell = c, Value = text, SheetName = sheetName };
                }
            }
        }
    }

}

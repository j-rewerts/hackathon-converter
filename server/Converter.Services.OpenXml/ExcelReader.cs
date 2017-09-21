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
                    var worksheetInfo = new Worksheet();
                    this.worksheets.Add(worksheetInfo);
                    foreach (SheetData sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                        foreach (var cellInfo in GetCellValues(workbookPart, sheetData, sheetName))
                            yield return cellInfo;
                    
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

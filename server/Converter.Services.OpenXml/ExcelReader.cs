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
        
        public ExcelReader(Stream file)
        {
            this.file = file;
        }
        public void ReadFile()//Action<object> onCellValueRead)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(this.file, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
                {
                    foreach (SheetData sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                    {
                        GetCellValues(sheetData);
                    }
                }
                //WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                //SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            }
        }
        public void GetCellValues(SheetData sheetData)
        {
            string text = "";
            foreach (Row r in sheetData.Elements<Row>())
            {
                foreach (Cell c in r.Elements<Cell>())
                {
                    text = c.CellValue.Text;
                    Console.Write(text + " ");
                }
            }
        }
    }

}

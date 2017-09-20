using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

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
                
            }
        }
    }

}

using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.OpenXml
{
    public class CellInfo
    {
        public string SheetName { get; set; }
        public Cell Cell { get; set; }
        public string Value { get; set; }
    }
}

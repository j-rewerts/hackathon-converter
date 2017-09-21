using DocumentFormat.OpenXml.Spreadsheet;

namespace Converter.Services.OpenXml
{
    public class CellInfo
    {
        public string SheetName { get; set; }
        public Cell Cell { get; set; }
        public string Reference { get; set; }
        public string Formula { get; set; }
        public string Value { get; set; }
    }
}

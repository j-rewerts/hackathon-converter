using System.Collections.Generic;

namespace Converter.Services.Data.Models
{
    internal class Workbook
    {
        public int WorkbookID { get; set; }
        public string GoogleID { get; set; }
        public string FileName { get; set; }

        public AnalysisStatus AnalysisStatus { get; set; }
        public List<Worksheet> Worksheets { get; set; }
        public List<Issue> Issues { get; set; } 
    }
}

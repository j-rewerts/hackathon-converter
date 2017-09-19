using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Workbook
    {
        [Key]
        public int WorkbookID { get; set; }
        public string GoogleID { get; set; }
        public string FileName { get; set; }

        public AnalysisStatus AnalysisStatus { get; set; }
        public List<Worksheet> Worksheets { get; set; }
        public List<Issue> Issues { get; set; } 
    }
}

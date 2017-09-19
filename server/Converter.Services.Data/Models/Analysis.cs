using Converter.Services.Data.Enums;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Analysis
    {
        [Key]
        public int AnalysisID { get; set; }

        public AnalysisStatus AnalysisStatus { get; set; }
        public Workbook Workbook { get; set; } 
        public List<Issue> Issues { get; set; }
    }
}

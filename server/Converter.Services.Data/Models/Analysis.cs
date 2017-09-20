using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Converter.Services.Data.Enums;

namespace Converter.Services.Data.Models
{
    internal class Analysis
    {
        [Key]
        public int AnalysisID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public AnalysisStatus AnalysisStatus { get; set; }
        public Workbook Workbook { get; set; } 
        public List<IssueBase> Issues { get; set; }
    }
}

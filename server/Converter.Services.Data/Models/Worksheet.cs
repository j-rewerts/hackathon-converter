using System.Collections.Generic;

namespace Converter.Services.Data.Models
{
    internal class Worksheet
    {
        public int WorksheetID { get; set; }
        public int RowCount { get; set; }
        public int CellCount { get; set; }

        public AnalysisStatus AnalysisStatus { get; set; }
        public List<Issue> Issues { get; set; }
    }
}

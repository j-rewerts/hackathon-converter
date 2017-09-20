using System.Collections.Generic;
using Converter.Services.Data.Enums;

namespace Converter.Services.Data.DTO
{
    public class AnalysisDto
    {
        public int AnalysisID { get; set; }
        public AnalysisStatus AnalysisStatus { get; set; }
        public string WorkbookName { get; set; }
        public List<IssueDto> Issues { get; set; }
    }
}

using System.Collections.Generic;
using Converter.Services.Data.Enums;

namespace Converter.Services.Data.DTO
{
    public class AnalysisDto
    {
        public int Id { get; set; }
        public string GoogleFileId { get; set; }        
        public string FileName { get; set; }
        public AnalysisStatus Status { get; set; }
        public List<IssueDto> Issues { get; set; }
    }
}

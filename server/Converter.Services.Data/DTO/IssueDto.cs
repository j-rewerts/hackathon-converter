using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data.DTO
{
    public class IssueDto
    {
        public int IssueID { get; set; }
        public string IssueType { get; set; }
        public string Message { get; set; }
    }
}

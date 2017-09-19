using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data.Models
{
    public class Issue
    {
        public int IssueID { get; set; }
        public string CellReference { get; set; }

        public IssueType IssueType { get; set; }
    }
}

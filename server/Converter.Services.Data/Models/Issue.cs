using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data.Models
{
    internal class Issue
    {
        public int IssueID { get; set; }
        public string CellReference { get; set; }

        public IssueType IssueType { get; set; }
    }
}

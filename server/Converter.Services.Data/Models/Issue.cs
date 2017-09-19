using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Issue
    {
        [Key]
        public int IssueID { get; set; }
        public string CellReference { get; set; }

        public IssueType IssueType { get; set; }
    }
}

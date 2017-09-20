using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Converter.Services.Data.Models
{
    [Table("Issue")]
    internal abstract class IssueBase
    {
        [Key]
        public int IssueID { get; set; }
        public string Message { get; set; }

        public IssueType IssueType { get; set; }
    }
}

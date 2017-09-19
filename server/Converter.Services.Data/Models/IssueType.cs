using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class IssueType
    {
        [Key]
        public int IssueTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

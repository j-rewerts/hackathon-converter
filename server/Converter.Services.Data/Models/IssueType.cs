using System.ComponentModel.DataAnnotations;
using Converter.Services.Data.Enums;

namespace Converter.Services.Data.Models
{
    internal class IssueType
    {
        [Key]
        public int IssueTypeID { get; set; }
        public string Description { get; set; }
        public bool ManualConversionRequired { get; set; }

        public IssueLevel IssueLevel { get; set; }
    }
}

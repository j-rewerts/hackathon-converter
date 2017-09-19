using Converter.Services.Data.Enums;
using System.ComponentModel.DataAnnotations;

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

using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Cell
    {
        [Key]
        public int CellID { get; set; }
        public string Reference { get; set; }
        public string Value { get; set; }
        public string Formula { get; set; }

        public Worksheet Worksheet { get; set; }
    }
}

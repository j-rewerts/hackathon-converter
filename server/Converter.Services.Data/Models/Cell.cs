using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Cell
    {
        [Key]
        public int CellID { get; set; }
        public int Row { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }

        public Worksheet Worksheet { get; set; }
    }
}

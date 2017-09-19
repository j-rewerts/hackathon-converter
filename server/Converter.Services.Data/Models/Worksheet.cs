using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Worksheet
    {
        [Key]
        public int WorksheetID { get; set; }
        public string Name { get; set; }
        public int RowCount { get; set; }
        public int CellCount { get; set; }
    }
}

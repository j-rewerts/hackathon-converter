using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    internal class Workbook
    {
        [Key]
        public int WorkbookID { get; set; }
        public string Name { get; set; }
        public string GoogleFileID { get; set; }

        //public Analysis Analysis { get; set; }
        public List<Worksheet> Worksheets { get; set; }
    }
}

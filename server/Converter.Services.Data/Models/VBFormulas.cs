using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Converter.Services.Data.Models
{
    class VBFormulas
    {
        [Key]
        public int FormulaID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }

        public Workbook WorkBook { get; set; }
    }
}

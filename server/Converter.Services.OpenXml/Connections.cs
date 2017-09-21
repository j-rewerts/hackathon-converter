using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.OpenXml
{
    class Connections
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConnectionProperties ConnectionProperties { get; set; }
    }
}

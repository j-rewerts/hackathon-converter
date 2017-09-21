using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.OpenXml
{
    public class Workbook
    {
        public int WorkbookID { get; set; }
        public string Name { get; set; }
        public string GoogleFileID { get; set; }
        public bool HasExternalConnections { get; set; }
        public HashSet<string> ExternalRelationships { get; set; }
        public bool HasCustomCode { get; set; }
        public bool HasDataConnections { get; set; }
        public bool HasExternalHyperLinks { get; set; }
        public bool HasExternalRelationships { get; set; }
    }

}

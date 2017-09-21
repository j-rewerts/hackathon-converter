using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.OpenXml
{
    public class Worksheet
    {
        public string Name { get; set; }
        public uint FirstColumn { get; set; }
        public uint LastColumn { get; set; }
        public uint FirstRow { get; set; }
        public uint LastRow { get; set; }

        public uint CellCount
        {
            get
            {
                return (LastColumn - FirstColumn + 1) * (LastRow - FirstRow + 1);
            }
        }
    }
}

using Converter.Services.OpenXml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Converter.Services.Tests
{
    [TestClass]
    public class ExcelReaderTests
    {
        [TestMethod]
        public void ReadExcelSheet()
        {
            // open test excel file
            string excelFile = Path.Combine(
                Directory.GetCurrentDirectory(),
                "TestBook1.xlsx");

            using (var stream = new FileStream(path: excelFile, mode: FileMode.Open))
            {
                var reader = new ExcelReader(stream);
                reader.ReadFile();
            }
        }
    }
}

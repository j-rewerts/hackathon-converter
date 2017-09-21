using Converter.Services.OpenXml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

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
                var reader = new ExcelReader(stream, "ASDSA");
                reader.ReadFile().ToList();
            }
        }
        [TestMethod]
        public void ReadExternalExcelSheet()
        {
            // open test excel file
            //string excelFile = @"C:\Users\bnaka\Desktop\Finance\3883FormSearchRegistrationDischarge.xlsm";
            string excelFile = @"C:\Users\bnaka\Desktop\Finance\Linked to multiple excel.xlsx";
            using (var stream = new FileStream(path: excelFile, mode: FileMode.Open))
            {
                var reader = new ExcelReader(stream, "ASDASDASD_SADDSA");
                var allItems = reader.ReadFile().ToList();

                var EmptyOnes = allItems.Where(x => string.IsNullOrWhiteSpace(x.Value)).ToList();
                var nonEmptyOnes = allItems.Where(x => !string.IsNullOrWhiteSpace(x.Value)).ToList();
                var formulaOnes = allItems.Where(x => !string.IsNullOrWhiteSpace(x.Formula)).ToList();
                var cellCount = reader.GetWorksheets().Sum(x => x.CellCount);
                var sheetCount = reader.GetWorksheets().Count();
                var formulaCount = allItems.Count(x => !string.IsNullOrWhiteSpace(x.Formula));
                var test = "";
            }
        }
        [TestMethod]
        [ExpectedException(typeof(NotSpreadSheetException))]
        public void NotValidExcelFile()
        {
            // open test excel file
            //string excelFile = @"C:\Users\bnaka\Desktop\Finance\3883FormSearchRegistrationDischarge.xlsm";
            string excelFile = @"C:\Users\bnaka\Desktop\Finance\Linked to excel.xls";
            using (var stream = new FileStream(path: excelFile, mode: FileMode.Open))
            {
                var reader = new ExcelReader(stream, "ASDASDASD_SADDSA");
                var allItems = reader.ReadFile().ToList();

                var EmptyOnes = allItems.Where(x => string.IsNullOrWhiteSpace(x.Value)).ToList();
                var nonEmptyOnes = allItems.Where(x => !string.IsNullOrWhiteSpace(x.Value)).ToList();
                var formulaOnes = allItems.Where(x => !string.IsNullOrWhiteSpace(x.Formula)).ToList();
                var cellCount = reader.GetWorksheets().Sum(x => x.CellCount);
                var sheetCount = reader.GetWorksheets().Count();
                var formulaCount = allItems.Count(x => !string.IsNullOrWhiteSpace(x.Formula));
                var test = "";
            }
        }
    }
}

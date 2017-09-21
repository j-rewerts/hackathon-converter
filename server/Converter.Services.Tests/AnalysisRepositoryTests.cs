using Converter.Services.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Converter.Services.Tests
{
    [TestClass]
    public class AnalysisRepositoryTests
    {
        [TestMethod]
        public async Task StartAnalysisTest()
        {
            IAnalysisContext context = new MockAnalysisContext();
            AnalysisRepository repo = new AnalysisRepository(context);
            int id = await repo.StartAnalysisAsync("a-file-id");
            Assert.AreEqual(-1, id);
        }
    }
}

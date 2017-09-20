using Converter.Services.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Converter.Services.Tests
{
    [TestClass]
    public class AnalysisRepositoryTests
    {
        [TestMethod]
        public void StartAnalysisTest()
        {
            IAnalysisContext context = new MockAnalysisContext();
            //AnalysisRepository repo = new AnalysisRepository(context);
        }
    }
}

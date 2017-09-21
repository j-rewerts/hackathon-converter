using Converter.Services.TaskRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Converter.Services.Tests
{
    [TestClass]
    public class GDriveTests
    {
        private const string OAUTH_TOKEN = "ya29.GlvNBAIlkKfoOOcPLQM76rlOwATZmYN5275wmVrj1NuqpCYvlxqnv5TTHpPn1tscUMfpBJQPqrnBQ6XsH8KCnIwGnkQfaYtjZwQvJ4V7xrI8yfARmbGJbMiV6pty"; // update whenever you run the tests

        [TestMethod]
        public void ReadGDriveFile()
        {
            string fileId = "1TkvTphttznBg9t4zR29gATu9WEhuAetygjLueMAAqHE";
            byte[] startFileResults = new byte[256];
            ExcelAnalyzer.GetGoogleDriveFile(fileId, OAUTH_TOKEN, s =>
            {
                Assert.IsTrue(s.Read(startFileResults, 0, 256) > 0);
            });
        }
    }
}

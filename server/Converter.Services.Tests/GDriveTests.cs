using Converter.Services.TaskRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Services.Tests
{
    [TestClass]
    public class GDriveTests
    {
        private const string OAUTH_TOKEN = "ya29.GlvNBBkehEPR6cdyR20ZxwQrTTWiE7GQOv-qtkaTisL8k5KOlI5NdMMSeSbHCg9soqscDBVbaBydKxJnVxYp5mPNEd-JXuuyELJZGJGGIOlu_mWk-3JuWy44il9j";
        [TestMethod]
        public async Task ReadGDriveFile()
        {
            string fileId = "0BxmMOBL2mKJ9YjJ6WDhReXcxQVE";
            byte[] startFileResults = new byte[256];
            await ExcelAnalyzer.GetGoogleDriveFileAsync(fileId, OAUTH_TOKEN, async s =>
            {
                Assert.IsTrue(s.Read(startFileResults, 0, 256) > 0);
            });
        }
    }
}

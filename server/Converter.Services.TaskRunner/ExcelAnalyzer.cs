using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Converter.Services.OpenXml;
using Converter.Services.Data;

namespace Converter.Services.TaskRunner
{
    public class ExcelAnalyzer
    {


        public ExcelAnalyzer(
            IAnalysisRepository repository,
            ILogger<ExcelAnalyzer> logger
            )
        {
            if (logger == null)
                _logger = new Logger<ExcelAnalyzer>(new LoggerFactory());
            else
                _logger = logger;
            this._repository = repository;
        }

        private readonly ILogger<ExcelAnalyzer> _logger;
        private readonly IAnalysisRepository _repository;

        public void Analyze(string googleFileId, int analysisId, string oauthToken)
        {
            if (string.IsNullOrWhiteSpace(googleFileId))
                throw new ArgumentNullException("googleFileId");
            if (analysisId <= 0)
                throw new ArgumentOutOfRangeException("analysisId", "analysisId must be greater than 0");
            if (string.IsNullOrWhiteSpace(oauthToken))
                throw new ArgumentNullException("oauthToken");


            try
            {
                GetGoogleDriveFile(googleFileId, oauthToken, stream =>
                {
                    var reader = new ExcelReader(stream, googleFileId);

                    foreach (var cell in reader.ReadFile())
                    {

                    }

                    // now that we've read the workbook we can save the analysis results
                    foreach (var sheetInfo in reader.GetWorksheets())
                    {
                        //repository.AddWorksheetAsync(analysisId, sheetInfo.Name, sheetInfo.RowCount, sheetInfo.ColumnCount * sheetInfo.RowCount);
                    }
                });
            }
            catch (Exception err)
            {
                _logger.LogError(1, err, $"Unable to get Google drive file: { err.Message}");
            }
        }

        internal static void GetGoogleDriveFile(string id, string oauthToken, Action<Stream> callback)
        {
            string applicationName = "Google File Checker"; //_configuration["Google:ApplicationName"];
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new InvalidOperationException("applicationName is missing");
            }
            else
            { 
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = new GoogleAuthenticator(oauthToken),
                    ApplicationName = applicationName
                });

                var file = service.Files.Get(id);
                string tempFile = Path.GetTempFileName();
                using (Stream s = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    file.Download(s);
                    s.Flush();
                }

                try
                {
                    using (var s = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                    {
                        callback(s);
                    }
                }
                finally
                {
                    // cleanup the temporary file
                    File.Delete(tempFile);
                }
            }
        }

       
    }
}

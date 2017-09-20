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
            ILogger<ExcelAnalyzer> logger,
            IConfigurationRoot configuration,
            IAnalysisRepository repository)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        private readonly ILogger<ExcelAnalyzer> _logger;
        private readonly IConfigurationRoot _configuration;
        private readonly IAnalysisRepository repository;

        public void Analyze(string googleFileId)
        {
            try
            {
                GetGoogleDriveFile(googleFileId, stream =>
                {
                    var reader = new ExcelReader(stream, googleFileId);

                    foreach (var cell in reader.ReadFile())
                    {

                    }

                    // now that we've read the workbook we can save the analysis results
                    var worksheets = reader.GetWorksheets();
                });
            }
            catch (Exception err)
            {
                _logger.LogError(1, err, $"Unable to get Google drive file: { err.Message}");
            }
        }

        private void GetGoogleDriveFile(string id, Action<Stream> callback)
        {
            string applicationName = _configuration["Google:ApplicationName"];
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = new GoogleAuthenticator(),
                    ApplicationName = applicationName
                });

                var file = service.Files.Get(id);
                string tempFile = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                using (Stream s = new FileStream(tempFile, FileMode.CreateNew, FileAccess.Write))
                {
                    file.Download(s);
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

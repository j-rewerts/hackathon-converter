﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Converter.Services.OpenXml;
using Converter.Services.Data;
using System.Threading.Tasks;

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

        public async Task AnalyzeAsync(string googleFileId, int analysisId)
        {
            try
            {
                await GetGoogleDriveFileAsync(googleFileId, async stream => { await AnalyzeAsync(googleFileId, stream); });
            }
            catch (Exception err)
            {
                _logger.LogError(1, err, $"Unable to get Google drive file: { err.Message}");
            }
        }

        public async Task AnalyzeAsync(string googleFileId, Stream stream)
        {
            var workbook = await _repository.RetrieveWorkbookByGoogleFileIdAsync(googleFileId);
            var reader = new ExcelReader(stream);

            await _repository.AddWorksheetsAsync(workbook.Id, reader.GetSheetNames());
            
            // now that we've read the workbook we can save the analysis results
            foreach (var cellInfo in reader.ReadFile())
            {
                await _repository.AddCellAsync(cellInfo.SheetName, cellInfo.Reference, cellInfo.Value, cellInfo.Formula);
            }
        }

        private async Task GetGoogleDriveFileAsync(string id, Func<Stream, Task> callback)
        {
            string applicationName = "Google File Checker"; //_configuration["Google:ApplicationName"];
            if (!string.IsNullOrWhiteSpace(applicationName))
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
                        await callback(s);
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

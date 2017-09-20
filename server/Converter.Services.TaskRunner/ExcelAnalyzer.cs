using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Converter.Services.TaskRunner
{
    public class ExcelAnalyzer
    {
        public ExcelAnalyzer(ILogger<ExcelAnalyzer> logger)
        {
            this._logger = logger;
        }

        private readonly ILogger<ExcelAnalyzer> _logger;


        public void Analyze(string googleFileId)
        {
            try
            {
                using (Stream stream = GetGoogleDriveFile(googleFileId))
                {
                    //var reader = new ExcelReader(stream, id);
                    //reader.ReadFile();
                }
            }
            catch (Exception err)
            {
                _logger.LogError(1, err, $"Unable to get Google drive file: { err.Message}");
            }
        }

        private Stream GetGoogleDriveFile(string id)
        {
            throw new NotImplementedException();
        }
    }
}

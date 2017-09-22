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
using System.Threading.Tasks;
using System.Linq;

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
        public ILogger<ExcelAnalyzer> Logger { get { return _logger; } }
        private readonly IAnalysisRepository _repository;
        

        public async Task AnalyzeAsync(string googleFileId, string oauthToken)
        {
            if (string.IsNullOrWhiteSpace(googleFileId))
                throw new ArgumentNullException("googleFileId");
            if (string.IsNullOrWhiteSpace(oauthToken))
                throw new ArgumentNullException("oauthToken");


            try
            {
                await GetGoogleDriveFileAsync(googleFileId, oauthToken, async stream => 
                {
                    
                    await AnalyzeAsync(googleFileId, stream);
                });
            }
            catch (Exception err)
            {
                _logger.LogError(1, err, $"Unable to get Google drive file: { err.Message}");
            }
        }

        public async Task AnalyzeAsync(string googleFileId, Stream stream)
        {
            Data.DTO.WorkbookDto workbook;
            try
            {
                workbook = await _repository.RetrieveWorkbookByGoogleFileIdAsync(googleFileId);
            }
            catch (Exception err)
            {
                throw new InvalidOperationException($"Unable to get workbook with googleFileId { googleFileId }", err);
            }
            ExcelReader reader;
            try
            {
                reader = new ExcelReader(stream);
            }
            catch (Exception err)
            {
                throw;
            }

            await _repository.AddWorksheetsAsync(workbook.Id, reader.GetSheetNames());
            
            // now that we've read the workbook we can save the analysis results
            foreach (var cellInfo in reader.ReadFile())
            {
                await _repository.AddCellAsync(cellInfo.SheetName, cellInfo.RowIndex, cellInfo.ColumnIndex, cellInfo.Reference, cellInfo.Value, cellInfo.Formula);
            }

            var readerWorkbook = reader.GetWorkbook();
            var readerWorksheets = reader.GetWorksheets();

            if (readerWorkbook.HasExternalConnections)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 1, "Has External Connections");
            if (readerWorkbook.HasCustomCode)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 2, "Has Custom Code");
            if (readerWorkbook.HasDataConnections)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 3, "Has Data Connections");
            if (readerWorkbook.HasExternalHyperLinks)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 4, "Has External Hyper Links");
            if (readerWorkbook.HasExternalRelationships)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 5, "Has External Relationships");

            uint rowCountTotal = 0;
            foreach (Worksheet worksheet in readerWorksheets)
                rowCountTotal += worksheet.CellCount;
            if (rowCountTotal > 2000000)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 6, string.Format("Row count exceeds 2,000,000. Row count is {0}", rowCountTotal));

            if (readerWorkbook.FormulaCount > 40000)
                await _repository.AddWorkbookIssueAsync(workbook.AnalysisId, 7, string.Format("Formula count exceeds 40,000. Formula count is {0}", formulaCount));
        }

        internal static async Task GetGoogleDriveFileAsync(string id, string oauthToken, Func<Stream, Task> callback)
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
                try
                {
                    using (Stream s = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var result = file.DownloadWithStatus(s);
                        if (result.Status != Google.Apis.Download.DownloadStatus.Completed)
                            throw new FileRetrievalFailedException("Unable to retrive file");
                   
                        s.Flush();
                    }

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

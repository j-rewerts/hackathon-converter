using Converter.Services.Data.Models;
using System.Threading.Tasks;
using System;

namespace Converter.Services.Data
{
    internal class AnalysisRepository : IAnalysisRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <remarks>context is typically provided through dependency injection</remarks>
        public AnalysisRepository(IAnalysisContext context)
        {
            this._context = context;
        }

        private readonly IAnalysisContext _context;

        public async Task<int> AddAnalysisAsync(string fileId)
        {
            var analysis = new Analysis();
            // TODO; set workbook properties here (maybe just fileId?)
            _context.Analysis.Add(analysis);
            await _context.SaveChangesAsync();

            return analysis.AnalysisID;
        }

        public async void StartAnalysisAsync(int analysisId)
        {
            var analysis = _context.Analysis.Find(analysisId);
            analysis.AnalysisStatus = Enums.AnalysisStatus.InProgress;
            await _context.SaveChangesAsync();
        }

        public async void CompleteAnalysisAsync(string analysisId)
        {
            throw new NotImplementedException("This function needs to be implemented.");
        }

        public async void AddWorkbookIssueAsync(string analysisId, string issueTypeId, string message)
        {
            throw new NotImplementedException("This function needs to be implemented.");
        }

        public async void AddWorksheetAsync(string workbookId, string name, int rowCount, int cellCount)
        {
            throw new NotImplementedException("This function needs to be implemented.");
        }

        public async void AddCellIssueAsync(string analysisId, string issueTypeId, string cellReference, string worksheet, string message)
        {
            throw new NotImplementedException("This function needs to be implemented.");
        }
    }
}

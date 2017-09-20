using System;
using System.Linq;
using System.Threading.Tasks;

using Converter.Services.Data.Models;

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

        public async Task<int> StartAnalysisAsync(string googleFileId)
        {
            var analysis = _context.Analysis.Find(googleFileId);
            analysis.AnalysisStatus = Enums.AnalysisStatus.InProgress;
            await _context.SaveChangesAsync();
            return analysis.AnalysisID;
        }

        public async Task<int> CompleteAnalysisAsync(int analysisId)
        {
            throw new NotImplementedException("This function needs to be implemented.");
        }

        public async Task<int> AddCellIssueAsync(int analysisId, int issueTypeId, int cellId, string message)
        {
            var analysis = _context.Analysis.FirstOrDefault(x => x.AnalysisID == analysisId);
            if (analysis is null)
                throw new Exception("Invalid Analysis ID provided.");
            var issueType = _context.IssueType.FirstOrDefault(x => x.IssueTypeID == issueTypeId);
            if (issueType is null)
                throw new Exception("Invalid Issue Type ID provided.");
            var cell = _context.Cell.FirstOrDefault(x => x.CellID == cellId);
            if (cell is null)
                throw new Exception("Invalid Cell ID provided.");

            var cellIssue = new CellIssue
            {
                IssueType = issueType,
                Message = message,
                Cell = cell
            };
            analysis.Issues.Add(cellIssue);

            await _context.SaveChangesAsync();
            return cellIssue.IssueID;
        }

        public async Task<int> AddWorkbookIssueAsync(int analysisId, int issueTypeId, string message)
        {
            var analysis = _context.Analysis.FirstOrDefault(x => x.AnalysisID == analysisId);
            if (analysis is null)
                throw new Exception("Invalid Analysis ID provided.");
            var issueType = _context.IssueType.FirstOrDefault(x => x.IssueTypeID == issueTypeId);
            if (issueType is null)
                throw new Exception("Invalid Issue Type ID provided.");

            var workbookIssue = new WorkbookIssue
            {
                IssueType = issueType,
                Message = message
            };
            analysis.Issues.Add(workbookIssue);

            await _context.SaveChangesAsync();
            return workbookIssue.IssueID;
        }

        public async Task<int> AddWorksheetAsync(int workbookId, string name, int rowCount, int cellCount)
        {
            var workbook = _context.Workbook.FirstOrDefault(x => x.WorkbookID == workbookId);
            if (workbook is null)
                throw new Exception("Invalid Workbook ID provided.");

            var worksheet = new Worksheet
            {
                Name = name,
                RowCount = rowCount,
                CellCount = cellCount
            };
            workbook.Worksheets.Add(worksheet);

            await _context.SaveChangesAsync();
            return worksheet.WorksheetID;
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper.QueryableExtensions;

using Converter.Services.Data.Models;
using Converter.Services.Data.DTO;
using Converter.Services.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Converter.Services.Data
{
    internal class AnalysisRepository : IAnalysisRepository, IDisposable
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

        public string ConnectionString
        {
            get
            {
                return (_context as AnalysisContext)?.Database?.GetDbConnection()?.ConnectionString;
            }
        }

        public async Task<int> StartAnalysisAsync(string googleFileId)
        {
            var workbook = _context.Workbook.FirstOrDefault(x => x.GoogleFileID == googleFileId);
            if (workbook is null)
            { 
                workbook = new Workbook { GoogleFileID = googleFileId };
                _context.Workbook.Add(workbook);
            }

            var analysis = _context.Analysis.FirstOrDefault(x => x.Workbook.WorkbookID == workbook.WorkbookID);
            if (analysis is null)
            {
                analysis = new Analysis
                {
                    Workbook = workbook,
                    AnalysisStatus = AnalysisStatus.InProgress,
                    StartDateTime = DateTime.Now
                };
                _context.Analysis.Add(analysis);
            }
            else
            {
                analysis.AnalysisStatus = AnalysisStatus.InProgress;
                analysis.StartDateTime = DateTime.Now;
                analysis.EndDateTime = null;
            }

            await _context.SaveChangesAsync();
            return analysis.AnalysisID;
        }

        public async Task CompleteAnalysisAsync(int analysisId)
        {
            var analysis = _context.Analysis.FirstOrDefault(x => x.AnalysisID == analysisId);
            if (analysis is null)
                throw new Exception("Invalid Analysis ID provided.");

            analysis.AnalysisStatus = AnalysisStatus.Completed;
            analysis.EndDateTime = DateTime.Now;

            await _context.SaveChangesAsync();
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

        public async Task AddWorksheetsAsync(int workbookId, IEnumerable<string> names)
        {
            var workbook = _context.Workbook.Include(x => x.Worksheets).FirstOrDefault(x => x.WorkbookID == workbookId);
            if (workbook is null)
                throw new Exception("Invalid Workbook ID provided.");

            foreach (var name in names)
                if (!workbook.Worksheets.Any(x => x.Name == name))
                    workbook.Worksheets.Add(new Worksheet { Name = name });

            await _context.SaveChangesAsync();
        }

        public async Task<int> AddCellAsync(string worksheetName, int rowIndex, int columnIndex, string reference, string value, string formula)
        {
            var worksheet = _context.Worksheet.Include(x => x.Cells).FirstOrDefault(x => x.Name == worksheetName);
            if (worksheet is null)
                throw new Exception("Invalid Worksheet Name provided.");

            Cell cell = worksheet.Cells.FirstOrDefault(x => x.Reference == reference); 
            if (cell is null)
            {
                cell = new Cell
                {
                    RowIndex = rowIndex,
                    ColumnIndex = columnIndex,
                    Reference = reference,
                    Value = value,
                    Formula = formula
                };
                worksheet.Cells.Add(cell);
                await _context.SaveChangesAsync();
            }

            return cell.CellID;
        }

        public async Task UpdateWorksheetCountsAsync(string name, int cellCount, int columnCount, int formulaCount, int rowCount)
        {
            var worksheet = _context.Worksheet.FirstOrDefault(x => x.Name == name);
            if (worksheet is null)
                throw new Exception("Invalid Worksheet Name provided.");

            worksheet.CellCount = cellCount;
            worksheet.ColumnCount = columnCount;
            worksheet.FormulaCount = formulaCount;
            worksheet.RowCount = rowCount;

            await _context.SaveChangesAsync();
        }

        public async Task<List<AnalysisDto>> RetrieveAnalysisesAsync()
        {
            // var analysises = await _context.Analysis.ProjectTo<AnalysisDto>().ToListAsync();
            var analysises = _context.Analysis
                .Select(x => new AnalysisDto()
                {
                    FileName  = x.Workbook.Name,
                    Id = x.AnalysisID,
                    GoogleFileId = x.Workbook.GoogleFileID,
                    Status =  x.AnalysisStatus,
                    Issues = x.Issues.Select(y => new IssueDto()
                    {
                        Id = y.IssueID,
                        Type = y.IssueType.Description,
                        Message = y.Message
                    }).ToList()
                }).ToList();

 

            if (analysises is null)
                return new List<AnalysisDto>();
            return analysises;
        }

        public async Task<AnalysisDto> RetrieveAnalysisByGoogleFileIdAsync(string googleFileId)
        {
            var analysis = await _context.Analysis.ProjectTo<AnalysisDto>().FirstOrDefaultAsync(x => x.GoogleFileId == googleFileId);
            if (analysis is null)
                throw new Exception("Invalid Google File Id provided.");
            return analysis;
        }

        public async Task<WorkbookDto> RetrieveWorkbookByGoogleFileIdAsync(string googleFileId)
        {
            var workbook = await _context.Workbook.ProjectTo<WorkbookDto>().FirstOrDefaultAsync(x => x.GoogleFileId == googleFileId);
            if (workbook is null)
                throw new Exception("Invalid Google File Id provided.");
            return workbook;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    var ctx = _context as IDisposable;
                    if (ctx != null)
                    {
                        ctx.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AnalysisRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

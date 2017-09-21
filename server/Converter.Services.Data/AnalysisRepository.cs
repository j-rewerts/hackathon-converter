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
            var workbook = _context.Workbook.FirstOrDefault(x => x.GoogleFileID == googleFileId);
            if (workbook is null)
            { 
                workbook = new Workbook { GoogleFileID = googleFileId };
                await _context.SaveChangesAsync();
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

        public async Task<int> AddWorksheetAsync(int workbookId, string name)
        {
            var workbook = _context.Workbook.FirstOrDefault(x => x.WorkbookID == workbookId);
            if (workbook is null)
                throw new Exception("Invalid Workbook ID provided.");

            var worksheet = new Worksheet { Name = name };
            workbook.Worksheets.Add(worksheet);

            await _context.SaveChangesAsync();
            return worksheet.WorksheetID;
        }

        public async Task UpdateWorksheetCountsAsync(int worksheetId, int cellCount, int columnCount, int formulaCount, int rowCount)
        {
            var worksheet = _context.Worksheet.FirstOrDefault(x => x.WorksheetID == worksheetId);
            if (worksheet is null)
                throw new Exception("Invalid Worksheet ID provided.");

            worksheet.CellCount = cellCount;
            worksheet.ColumnCount = columnCount;
            worksheet.FormulaCount = formulaCount;
            worksheet.RowCount = rowCount;

            await _context.SaveChangesAsync();
        }

        public async Task<List<AnalysisDto>> RetrieveAnalysisesAsync()
        {
            var analysises = await _context.Analysis.ProjectTo<AnalysisDto>().ToListAsync();
            if (analysises is null)
                return new List<AnalysisDto>();
            return analysises;
        }

        public async Task<AnalysisDto> RetrieveAnalysisByIdAsync(int analysisId)
        {
            var analysis = await _context.Analysis.ProjectTo<AnalysisDto>().FirstOrDefaultAsync(x => x.Id == analysisId);
            if (analysis is null)
                return new AnalysisDto();
            return analysis;
        }
    }
}

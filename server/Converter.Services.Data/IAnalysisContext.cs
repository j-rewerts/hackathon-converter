using Converter.Services.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Converter.Services.Data
{
    internal interface IAnalysisContext
    {
        DbSet<Analysis> Analysis { get; set; }
        DbSet<Cell> Cell { get; set; }
        DbSet<CellIssue> CellIssue { get; set; }
        DbSet<IssueType> IssueType { get; set; }
        DbSet<Workbook> Workbook { get; set; }
        DbSet<WorkbookIssue> WorkbookIssue { get; set; }
        DbSet<Worksheet> Worksheet { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}

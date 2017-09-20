using Converter.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Converter.Services.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Converter.Services.Tests
{
    class MockAnalysisContext : IAnalysisContext
    {
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<Cell> Cell { get; set; }
        public DbSet<CellIssue> CellIssue { get; set; }
        public DbSet<IssueType> IssueType { get; set; }
        public DbSet<Workbook> Workbook { get; set; }
        public DbSet<WorkbookIssue> WorkbookIssue { get; set; }
        public DbSet<Worksheet> Worksheet { get; set; }

        public async Task SaveChangesAync()
        {
            // mock implementation does nothing
        }

    }
}

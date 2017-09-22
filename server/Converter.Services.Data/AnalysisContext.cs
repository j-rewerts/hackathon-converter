using Converter.Services.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Converter.Services.Data
{
    internal class AnalysisContext : DbContext, IAnalysisContext
    {
        public AnalysisContext()
            : base()
        { }


        public AnalysisContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<Cell> Cell { get; set; }
        public DbSet<CellIssue> CellIssue { get; set; }        
        public DbSet<IssueType> IssueType { get; set; }
        public DbSet<Workbook> Workbook { get; set; }
        public DbSet<WorkbookIssue> WorkbookIssue { get; set; }
        public DbSet<Worksheet> Worksheet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO: setup fluent API stuff 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connectionString = "server=35.197.80.226;Database=Converter;Uid=root;Pwd=P@ssw0rd;"; //TODO get from configuration
            optionsBuilder.UseMySql(connectionString);
        }
    }
}

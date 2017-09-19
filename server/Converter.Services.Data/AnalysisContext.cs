using Converter.Services.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Converter.Services.Data
{
    internal class AnalysisContext : DbContext
    {
        public AnalysisContext()
            : base()
        { }

        public AnalysisContext(DbContextOptions<AnalysisContext> options)
            : base(options)
        { }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<Workbook> Workbooks { get; set; }
        public DbSet<Worksheet> Worksheets { get; set; }

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

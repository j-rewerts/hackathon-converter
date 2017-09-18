using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data
{
    public class AnalysisContext : DbContext
    {
        public AnalysisContext() : base() { }

        public DbSet<Workbook> Workbooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO: setup fluent API stuff 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connectionString = ""; //TODO get from configuration
            optionsBuilder.UseMySql(connectionString);
        }
    }
}

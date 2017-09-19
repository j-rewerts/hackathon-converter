﻿using Converter.Services.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Converter.Services.Data
{
    public class AnalysisContext : DbContext
    {
        public AnalysisContext(DbContextOptions<AnalysisContext> options)
            : base(options)
        { }

        public DbSet<AnalysisStatus> AnalysisStatuses { get; set; }
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

            string connectionString = ""; //TODO get from configuration
            optionsBuilder.UseMySql(connectionString);
        }
    }
}

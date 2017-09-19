using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Converter.Services.Data;
using Converter.Services.Data.Enums;

namespace Converter.Services.Data.Migrations
{
    [DbContext(typeof(AnalysisContext))]
    [Migration("20170919171114_ReplaceWorkbookObj")]
    partial class ReplaceWorkbookObj
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Converter.Services.Data.Models.Issue", b =>
                {
                    b.Property<int>("IssueID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CellReference");

                    b.Property<int?>("IssueTypeID");

                    b.Property<int?>("WorkbookID");

                    b.Property<int?>("WorksheetID");

                    b.HasKey("IssueID");

                    b.HasIndex("IssueTypeID");

                    b.HasIndex("WorkbookID");

                    b.HasIndex("WorksheetID");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.IssueType", b =>
                {
                    b.Property<int>("IssueTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("IssueTypeID");

                    b.ToTable("IssueTypes");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Workbook", b =>
                {
                    b.Property<int>("WorkbookID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnalysisStatus");

                    b.Property<string>("FileName");

                    b.Property<string>("GoogleID");

                    b.HasKey("WorkbookID");

                    b.ToTable("Workbooks");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Worksheet", b =>
                {
                    b.Property<int>("WorksheetID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnalysisStatus");

                    b.Property<int>("CellCount");

                    b.Property<string>("Name");

                    b.Property<int>("RowCount");

                    b.Property<int?>("WorkbookID");

                    b.HasKey("WorksheetID");

                    b.HasIndex("WorkbookID");

                    b.ToTable("Worksheets");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Issue", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.IssueType", "IssueType")
                        .WithMany()
                        .HasForeignKey("IssueTypeID");

                    b.HasOne("Converter.Services.Data.Models.Workbook")
                        .WithMany("Issues")
                        .HasForeignKey("WorkbookID");

                    b.HasOne("Converter.Services.Data.Models.Worksheet")
                        .WithMany("Issues")
                        .HasForeignKey("WorksheetID");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Worksheet", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.Workbook")
                        .WithMany("Worksheets")
                        .HasForeignKey("WorkbookID");
                });
        }
    }
}

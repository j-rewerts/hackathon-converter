using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Converter.Services.Data;

namespace Converter.Services.Data.Migrations
{
    [DbContext(typeof(AnalysisContext))]
    [Migration("20170919162004_AddingTables")]
    partial class AddingTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Converter.Services.Data.Models.AnalysisStatus", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AnalysisStatuses");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Issue", b =>
                {
                    b.Property<int>("IssueID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CellReference");

                    b.Property<int?>("IssueTypeID");

                    b.Property<int?>("WorksheetID");

                    b.HasKey("IssueID");

                    b.HasIndex("IssueTypeID");

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

            modelBuilder.Entity("Converter.Services.Data.Models.Worksheet", b =>
                {
                    b.Property<int>("WorksheetID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AnalysisStatusId");

                    b.Property<int>("CellCount");

                    b.Property<int>("RowCount");

                    b.HasKey("WorksheetID");

                    b.HasIndex("AnalysisStatusId");

                    b.ToTable("Worksheets");
                });

            modelBuilder.Entity("Converter.Services.Data.Workbook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Filename");

                    b.HasKey("Id");

                    b.ToTable("Workbooks");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Issue", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.IssueType", "IssueType")
                        .WithMany()
                        .HasForeignKey("IssueTypeID");

                    b.HasOne("Converter.Services.Data.Models.Worksheet")
                        .WithMany("Issues")
                        .HasForeignKey("WorksheetID");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Worksheet", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.AnalysisStatus", "AnalysisStatus")
                        .WithMany()
                        .HasForeignKey("AnalysisStatusId");
                });
        }
    }
}

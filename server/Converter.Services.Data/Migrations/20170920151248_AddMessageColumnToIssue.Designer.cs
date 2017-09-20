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
    [Migration("20170920151248_AddMessageColumnToIssue")]
    partial class AddMessageColumnToIssue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Converter.Services.Data.Models.Analysis", b =>
                {
                    b.Property<int>("AnalysisID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnalysisStatus");

                    b.Property<DateTime>("EndDateTime");

                    b.Property<DateTime>("StartDateTime");

                    b.Property<int?>("WorkbookID");

                    b.HasKey("AnalysisID");

                    b.HasIndex("WorkbookID");

                    b.ToTable("Analysis");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.IssueBase", b =>
                {
                    b.Property<int>("IssueID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AnalysisID");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("IssueTypeID");

                    b.Property<string>("Message");

                    b.HasKey("IssueID");

                    b.HasIndex("AnalysisID");

                    b.HasIndex("IssueTypeID");

                    b.ToTable("Issue");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IssueBase");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.IssueType", b =>
                {
                    b.Property<int>("IssueTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("IssueLevel");

                    b.Property<bool>("ManualConversionRequired");

                    b.HasKey("IssueTypeID");

                    b.ToTable("IssueType");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Workbook", b =>
                {
                    b.Property<int>("WorkbookID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GoogleFileID");

                    b.Property<string>("Name");

                    b.HasKey("WorkbookID");

                    b.ToTable("Workbook");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Worksheet", b =>
                {
                    b.Property<int>("WorksheetID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CellCount");

                    b.Property<string>("Name");

                    b.Property<int>("RowCount");

                    b.Property<int?>("WorkbookID");

                    b.HasKey("WorksheetID");

                    b.HasIndex("WorkbookID");

                    b.ToTable("Worksheet");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.CellIssue", b =>
                {
                    b.HasBaseType("Converter.Services.Data.Models.IssueBase");

                    b.Property<string>("CellReference");

                    b.Property<int?>("WorksheetID");

                    b.HasIndex("WorksheetID");

                    b.ToTable("Issue");

                    b.HasDiscriminator().HasValue("CellIssue");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.WorkbookIssue", b =>
                {
                    b.HasBaseType("Converter.Services.Data.Models.IssueBase");


                    b.ToTable("Issue");

                    b.HasDiscriminator().HasValue("WorkbookIssue");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Analysis", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.Workbook", "Workbook")
                        .WithMany()
                        .HasForeignKey("WorkbookID");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.IssueBase", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.Analysis")
                        .WithMany("Issues")
                        .HasForeignKey("AnalysisID");

                    b.HasOne("Converter.Services.Data.Models.IssueType", "IssueType")
                        .WithMany()
                        .HasForeignKey("IssueTypeID");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.Worksheet", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.Workbook")
                        .WithMany("Worksheets")
                        .HasForeignKey("WorkbookID");
                });

            modelBuilder.Entity("Converter.Services.Data.Models.CellIssue", b =>
                {
                    b.HasOne("Converter.Services.Data.Models.Worksheet", "Worksheet")
                        .WithMany()
                        .HasForeignKey("WorksheetID");
                });
        }
    }
}

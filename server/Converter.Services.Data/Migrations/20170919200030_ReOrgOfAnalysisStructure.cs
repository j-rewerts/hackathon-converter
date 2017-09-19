using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class ReOrgOfAnalysisStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Workbooks_WorkbookID",
                table: "Issues");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_WorkbookID",
                table: "Issues",
                newName: "IX_Issues_AnalysisID");

            migrationBuilder.RenameColumn(
                name: "GoogleID",
                table: "Workbooks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Workbooks",
                newName: "GoogleFileID");

            migrationBuilder.RenameColumn(
                name: "WorkbookID",
                table: "Issues",
                newName: "AnalysisID");

            migrationBuilder.DropColumn(
                name: "AnalysisStatus",
                table: "Worksheets");

            migrationBuilder.DropColumn(
                name: "AnalysisStatus",
                table: "Workbooks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "IssueTypes");

            migrationBuilder.CreateTable(
                name: "Analysises",
                columns: table => new
                {
                    AnalysisID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AnalysisStatus = table.Column<int>(nullable: false),
                    WorkbookID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analysises", x => x.AnalysisID);
                    table.ForeignKey(
                        name: "FK_Analysises_Workbooks_WorkbookID",
                        column: x => x.WorkbookID,
                        principalTable: "Workbooks",
                        principalColumn: "WorkbookID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "IssueLevel",
                table: "IssueTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ManualConversionRequired",
                table: "IssueTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Analysises_WorkbookID",
                table: "Analysises",
                column: "WorkbookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Analysises_AnalysisID",
                table: "Issues",
                column: "AnalysisID",
                principalTable: "Analysises",
                principalColumn: "AnalysisID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Analysises_AnalysisID",
                table: "Issues");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_AnalysisID",
                table: "Issues",
                newName: "IX_Issues_WorkbookID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Workbooks",
                newName: "GoogleID");

            migrationBuilder.RenameColumn(
                name: "GoogleFileID",
                table: "Workbooks",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "AnalysisID",
                table: "Issues",
                newName: "WorkbookID");

            migrationBuilder.DropColumn(
                name: "IssueLevel",
                table: "IssueTypes");

            migrationBuilder.DropColumn(
                name: "ManualConversionRequired",
                table: "IssueTypes");

            migrationBuilder.DropTable(
                name: "Analysises");

            migrationBuilder.AddColumn<int>(
                name: "AnalysisStatus",
                table: "Worksheets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnalysisStatus",
                table: "Workbooks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "IssueTypes",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Workbooks_WorkbookID",
                table: "Issues",
                column: "WorkbookID",
                principalTable: "Workbooks",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

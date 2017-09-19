using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class AddingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypes",
                columns: table => new
                {
                    IssueTypeID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypes", x => x.IssueTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Worksheets",
                columns: table => new
                {
                    WorksheetID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AnalysisStatusId = table.Column<int>(nullable: true),
                    CellCount = table.Column<int>(nullable: false),
                    RowCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worksheets", x => x.WorksheetID);
                    table.ForeignKey(
                        name: "FK_Worksheets_AnalysisStatuses_AnalysisStatusId",
                        column: x => x.AnalysisStatusId,
                        principalTable: "AnalysisStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CellReference = table.Column<string>(nullable: true),
                    IssueTypeID = table.Column<int>(nullable: true),
                    WorksheetID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueID);
                    table.ForeignKey(
                        name: "FK_Issues_IssueTypes_IssueTypeID",
                        column: x => x.IssueTypeID,
                        principalTable: "IssueTypes",
                        principalColumn: "IssueTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_Worksheets_WorksheetID",
                        column: x => x.WorksheetID,
                        principalTable: "Worksheets",
                        principalColumn: "WorksheetID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_IssueTypeID",
                table: "Issues",
                column: "IssueTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_WorksheetID",
                table: "Issues",
                column: "WorksheetID");

            migrationBuilder.CreateIndex(
                name: "IX_Worksheets_AnalysisStatusId",
                table: "Worksheets",
                column: "AnalysisStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "IssueTypes");

            migrationBuilder.DropTable(
                name: "Worksheets");

            migrationBuilder.DropTable(
                name: "AnalysisStatuses");
        }
    }
}

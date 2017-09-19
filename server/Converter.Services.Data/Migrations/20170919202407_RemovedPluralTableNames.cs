using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class RemovedPluralTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.RenameTable(
                name: "Worksheets",
                newName: "Worksheet");

            migrationBuilder.RenameTable(
                name: "Workbooks",
                newName: "Workbook");

            migrationBuilder.RenameTable(
                name: "IssueTypes",
                newName: "IssueType");

            migrationBuilder.RenameTable(
                name: "Issues",
                newName: "Issue");

            migrationBuilder.RenameTable(
                name: "Analysises",
                newName: "Analysis");

            migrationBuilder.DropForeignKey(
                name: "FK_Analysises_Workbooks_WorkbookID",
                table: "Analysis");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Analysises_AnalysisID",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_IssueTypes_IssueTypeID",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Worksheets_WorksheetID",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Worksheets_Workbooks_WorkbookID",
                table: "Worksheet");

            migrationBuilder.RenameIndex(
                name: "IX_Worksheets_WorkbookID",
                table: "Worksheet",
                newName: "IX_Worksheet_WorkbookID");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_WorksheetID",
                table: "Issue",
                newName: "IX_Issue_WorksheetID");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_IssueTypeID",
                table: "Issue",
                newName: "IX_Issue_IssueTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_AnalysisID",
                table: "Issue",
                newName: "IX_Issue_AnalysisID");

            migrationBuilder.RenameIndex(
                name: "IX_Analysises_WorkbookID",
                table: "Analysis",
                newName: "IX_Analysis_WorkbookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Analysis_Workbook_WorkbookID",
                table: "Analysis",
                column: "WorkbookID",
                principalTable: "Workbook",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Analysis_AnalysisID",
                table: "Issue",
                column: "AnalysisID",
                principalTable: "Analysis",
                principalColumn: "AnalysisID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_IssueType_IssueTypeID",
                table: "Issue",
                column: "IssueTypeID",
                principalTable: "IssueType",
                principalColumn: "IssueTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Worksheet_WorksheetID",
                table: "Issue",
                column: "WorksheetID",
                principalTable: "Worksheet",
                principalColumn: "WorksheetID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worksheet_Workbook_WorkbookID",
                table: "Worksheet",
                column: "WorkbookID",
                principalTable: "Workbook",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analysis_Workbook_WorkbookID",
                table: "Analysises");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Analysis_AnalysisID",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_IssueType_IssueTypeID",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Worksheet_WorksheetID",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Worksheet_Workbook_WorkbookID",
                table: "Worksheets");

            migrationBuilder.RenameTable(
                name: "Worksheet",
                newName: "Worksheets");

            migrationBuilder.RenameTable(
                name: "Workbook",
                newName: "Workbooks");

            migrationBuilder.RenameTable(
                name: "IssueType",
                newName: "IssueTypes");

            migrationBuilder.RenameTable(
                name: "Issue",
                newName: "Issues");

            migrationBuilder.RenameTable(
                name: "Analysis",
                newName: "Analysises");

            migrationBuilder.RenameIndex(
                name: "IX_Worksheet_WorkbookID",
                table: "Worksheetss",
                newName: "IX_Worksheets_WorkbookID");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_WorksheetID",
                table: "Issuess",
                newName: "IX_Issues_WorksheetID");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_IssueTypeID",
                table: "Issuess",
                newName: "IX_Issues_IssueTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_AnalysisID",
                table: "Issuess",
                newName: "IX_Issues_AnalysisID");

            migrationBuilder.RenameIndex(
                name: "IX_Analysis_WorkbookID",
                table: "Analysiseses",
                newName: "IX_Analysises_WorkbookID");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Worksheet",
                table: "Worksheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workbook",
                table: "Workbooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueType",
                table: "IssuesTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issue",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Analysis",
                table: "Analysises");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Worksheets",
                table: "Worksheetss",
                column: "WorksheetID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workbooks",
                table: "Workbookss",
                column: "WorkbookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssuesTypess",
                column: "IssueTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issues",
                table: "Issuess",
                column: "IssueID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Analysises",
                table: "Analysiseses",
                column: "AnalysisID");

            migrationBuilder.AddForeignKey(
                name: "FK_Analysises_Workbooks_WorkbookID",
                table: "Analysiseses",
                column: "WorkbookID",
                principalTable: "Workbooks",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Analysises_AnalysisID",
                table: "Issuess",
                column: "AnalysisID",
                principalTable: "Analysises",
                principalColumn: "AnalysisID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_IssueTypes_IssueTypeID",
                table: "Issuess",
                column: "IssueTypeID",
                principalTable: "IssueTypes",
                principalColumn: "IssueTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Worksheets_WorksheetID",
                table: "Issuess",
                column: "WorksheetID",
                principalTable: "Worksheets",
                principalColumn: "WorksheetID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worksheets_Workbooks_WorkbookID",
                table: "Worksheetss",
                column: "WorkbookID",
                principalTable: "Workbooks",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

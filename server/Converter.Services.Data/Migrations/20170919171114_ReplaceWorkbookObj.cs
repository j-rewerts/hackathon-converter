using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class ReplaceWorkbookObj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "Workbooks",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Workbooks",
                newName: "WorkbookID");

            migrationBuilder.AddColumn<int>(
                name: "WorkbookID",
                table: "Worksheets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkbookID",
                table: "Issues",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnalysisStatus",
                table: "Workbooks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GoogleID",
                table: "Workbooks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Worksheets_WorkbookID",
                table: "Worksheets",
                column: "WorkbookID");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_WorkbookID",
                table: "Issues",
                column: "WorkbookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Workbooks_WorkbookID",
                table: "Issues",
                column: "WorkbookID",
                principalTable: "Workbooks",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worksheets_Workbooks_WorkbookID",
                table: "Worksheets",
                column: "WorkbookID",
                principalTable: "Workbooks",
                principalColumn: "WorkbookID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Workbooks_WorkbookID",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Worksheets_Workbooks_WorkbookID",
                table: "Worksheets");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Workbooks",
                newName: "Filename");

            migrationBuilder.RenameColumn(
                name: "WorkbookID",
                table: "Workbooks",
                newName: "Id");

            migrationBuilder.DropIndex(
                name: "IX_Worksheets_WorkbookID",
                table: "Worksheets");

            migrationBuilder.DropIndex(
                name: "IX_Issues_WorkbookID",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "WorkbookID",
                table: "Worksheets");

            migrationBuilder.DropColumn(
                name: "WorkbookID",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AnalysisStatus",
                table: "Workbooks");

            migrationBuilder.DropColumn(
                name: "GoogleID",
                table: "Workbooks");
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class AddCellTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Worksheet_WorksheetID",
                table: "Issue");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_WorksheetID",
                table: "Issue",
                newName: "IX_Issue_CellID");

            migrationBuilder.RenameColumn(
                name: "WorksheetID",
                table: "Issue",
                newName: "CellID");

            migrationBuilder.DropColumn(
                name: "CellReference",
                table: "Issue");

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    CellID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Column = table.Column<string>(nullable: true),
                    Row = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    WorksheetID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.CellID);
                    table.ForeignKey(
                        name: "FK_Cell_Worksheet_WorksheetID",
                        column: x => x.WorksheetID,
                        principalTable: "Worksheet",
                        principalColumn: "WorksheetID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cell_WorksheetID",
                table: "Cell",
                column: "WorksheetID");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Cell_CellID",
                table: "Issue",
                column: "CellID",
                principalTable: "Cell",
                principalColumn: "CellID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Cell_CellID",
                table: "Issue");

            migrationBuilder.RenameIndex(
                name: "IX_Issue_CellID",
                table: "Issue",
                newName: "IX_Issue_WorksheetID");

            migrationBuilder.RenameColumn(
                name: "CellID",
                table: "Issue",
                newName: "WorksheetID");

            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.AddColumn<string>(
                name: "CellReference",
                table: "Issue",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Worksheet_WorksheetID",
                table: "Issue",
                column: "WorksheetID",
                principalTable: "Worksheet",
                principalColumn: "WorksheetID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class AddedCountColumnsToWorksheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnCount",
                table: "Worksheet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormulaCount",
                table: "Worksheet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCustomCode",
                table: "Workbook",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDataConnections",
                table: "Workbook",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Column",
                table: "Cell",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "Analysis",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnCount",
                table: "Worksheet");

            migrationBuilder.DropColumn(
                name: "FormulaCount",
                table: "Worksheet");

            migrationBuilder.DropColumn(
                name: "HasCustomCode",
                table: "Workbook");

            migrationBuilder.DropColumn(
                name: "HasDataConnections",
                table: "Workbook");

            migrationBuilder.AlterColumn<string>(
                name: "Column",
                table: "Cell",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "Analysis",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}

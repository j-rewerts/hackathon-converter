using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class AlteredCellReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column",
                table: "Cell");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "Cell");

            migrationBuilder.AddColumn<string>(
                name: "Formula",
                table: "Cell",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Cell",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formula",
                table: "Cell");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Cell");

            migrationBuilder.AddColumn<int>(
                name: "Column",
                table: "Cell",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "Cell",
                nullable: false,
                defaultValue: 0);
        }
    }
}

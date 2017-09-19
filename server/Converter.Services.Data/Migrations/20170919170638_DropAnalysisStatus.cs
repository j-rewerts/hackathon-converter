using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Converter.Services.Data.Migrations
{
    public partial class DropAnalysisStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worksheets_AnalysisStatuses_AnalysisStatusId",
                table: "Worksheets");

            migrationBuilder.DropIndex(
                name: "IX_Worksheets_AnalysisStatusId",
                table: "Worksheets");

            migrationBuilder.DropColumn(
                name: "AnalysisStatusId",
                table: "Worksheets");

            migrationBuilder.DropTable(
                name: "AnalysisStatuses");

            migrationBuilder.AddColumn<int>(
                name: "AnalysisStatus",
                table: "Worksheets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalysisStatus",
                table: "Worksheets");

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

            migrationBuilder.AddColumn<int>(
                name: "AnalysisStatusId",
                table: "Worksheets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Worksheets_AnalysisStatusId",
                table: "Worksheets",
                column: "AnalysisStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Worksheets_AnalysisStatuses_AnalysisStatusId",
                table: "Worksheets",
                column: "AnalysisStatusId",
                principalTable: "AnalysisStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class FixingNamesInExam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "Exams");

            migrationBuilder.AddColumn<DateTime>(
                name: "Close",
                table: "Exams",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Open",
                table: "Exams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Close",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "Exams");

            migrationBuilder.AddColumn<DateTime>(
                name: "CloseTime",
                table: "Exams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpenTime",
                table: "Exams",
                type: "datetime2",
                nullable: true);
        }
    }
}

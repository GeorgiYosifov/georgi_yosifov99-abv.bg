using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class ChangingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "OnlineTests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingTime",
                table: "OnlineTests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingTime",
                table: "OnlineTests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "Mark",
                table: "Grades",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "SumOfPoints",
                table: "Grades",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrueAnswers",
                table: "Grades",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Points",
                table: "Answers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "EndingTime",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "StartingTime",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "SumOfPoints",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "TrueAnswers",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Answers");

            migrationBuilder.AlterColumn<int>(
                name: "Mark",
                table: "Grades",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class ChangingOnlineTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeRanges",
                table: "OnlineTests");

            migrationBuilder.AddColumn<double>(
                name: "MaxPoints",
                table: "OnlineTests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinPointsFor3",
                table: "OnlineTests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Range",
                table: "OnlineTests",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "MinPointsFor3",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "Range",
                table: "OnlineTests");

            migrationBuilder.AddColumn<string>(
                name: "GradeRanges",
                table: "OnlineTests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

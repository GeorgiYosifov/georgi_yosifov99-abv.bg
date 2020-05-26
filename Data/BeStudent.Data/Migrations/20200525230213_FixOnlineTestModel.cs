using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class FixOnlineTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InnerEndTimeForUser",
                table: "OnlineTests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InnerEndTimeForUser",
                table: "OnlineTests");
        }
    }
}

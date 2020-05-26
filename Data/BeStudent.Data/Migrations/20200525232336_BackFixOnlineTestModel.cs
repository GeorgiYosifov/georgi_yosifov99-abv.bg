using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class BackFixOnlineTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InnerEndTimeForUser",
                table: "OnlineTests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InnerEndTimeForUser",
                table: "OnlineTests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class FixingOnlineTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_OnlineTests_OnlineTestId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_OnlineTestId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "OnlineTestId",
                table: "Grades");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnlineTestId",
                table: "Grades",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_OnlineTestId",
                table: "Grades",
                column: "OnlineTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_OnlineTests_OnlineTestId",
                table: "Grades",
                column: "OnlineTestId",
                principalTable: "OnlineTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

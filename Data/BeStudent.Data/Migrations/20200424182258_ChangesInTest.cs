using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class ChangesInTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumOfPoints",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "TrueAnswers",
                table: "Grades");

            migrationBuilder.AddColumn<int>(
                name: "OnlineTestId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OnlineTestId",
                table: "AspNetUsers",
                column: "OnlineTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OnlineTests_OnlineTestId",
                table: "AspNetUsers",
                column: "OnlineTestId",
                principalTable: "OnlineTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OnlineTests_OnlineTestId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OnlineTestId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OnlineTestId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "SumOfPoints",
                table: "Grades",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrueAnswers",
                table: "Grades",
                type: "int",
                nullable: true);
        }
    }
}

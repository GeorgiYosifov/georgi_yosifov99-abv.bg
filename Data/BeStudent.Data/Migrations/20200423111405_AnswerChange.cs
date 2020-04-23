using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class AnswerChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AnswerId",
                table: "AspNetUsers",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Answers_AnswerId",
                table: "AspNetUsers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Answers_AnswerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AnswerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "AspNetUsers");
        }
    }
}

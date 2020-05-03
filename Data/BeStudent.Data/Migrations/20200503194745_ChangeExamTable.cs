using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class ChangeExamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ExamId",
                table: "AspNetUsers",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Exams_ExamId",
                table: "AspNetUsers",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Exams_ExamId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ExamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class ChangingNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineTests_Exams_TestId",
                table: "OnlineTests");

            migrationBuilder.DropIndex(
                name: "IX_OnlineTests_TestId",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "OnlineTests");

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "OnlineTests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_ExamId",
                table: "OnlineTests",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineTests_Exams_ExamId",
                table: "OnlineTests",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineTests_Exams_ExamId",
                table: "OnlineTests");

            migrationBuilder.DropIndex(
                name: "IX_OnlineTests_ExamId",
                table: "OnlineTests");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "OnlineTests");

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "OnlineTests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_TestId",
                table: "OnlineTests",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineTests_Exams_TestId",
                table: "OnlineTests",
                column: "TestId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

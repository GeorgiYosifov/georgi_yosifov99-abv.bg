using Microsoft.EntityFrameworkCore.Migrations;

namespace BeStudent.Data.Migrations
{
    public partial class FixingDecisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Answers_AnswerId",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_AnswerId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Decisions");

            migrationBuilder.AddColumn<double>(
                name: "Points",
                table: "Decisions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionId",
                table: "Decisions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_QuestionId",
                table: "Decisions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Questions_QuestionId",
                table: "Decisions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Questions_QuestionId",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_QuestionId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Decisions");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Decisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_AnswerId",
                table: "Decisions",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Answers_AnswerId",
                table: "Decisions",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

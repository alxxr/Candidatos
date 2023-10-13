using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.Migrations
{
    /// <inheritdoc />
    public partial class FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_candidateExperiences_candidates_IdCandidate",
                table: "candidateExperiences");

            migrationBuilder.AlterColumn<int>(
                name: "IdCandidate",
                table: "candidateExperiences",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_candidateExperiences_candidates_IdCandidate",
                table: "candidateExperiences",
                column: "IdCandidate",
                principalTable: "candidates",
                principalColumn: "IdCandidate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_candidateExperiences_candidates_IdCandidate",
                table: "candidateExperiences");

            migrationBuilder.AlterColumn<int>(
                name: "IdCandidate",
                table: "candidateExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_candidateExperiences_candidates_IdCandidate",
                table: "candidateExperiences",
                column: "IdCandidate",
                principalTable: "candidates",
                principalColumn: "IdCandidate",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

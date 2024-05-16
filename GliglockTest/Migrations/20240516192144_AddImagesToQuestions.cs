using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GliglockTest.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesToQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WithImg",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithImg",
                table: "Questions");
        }
    }
}

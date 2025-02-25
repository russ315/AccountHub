using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexToNameFieldOfGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                table: "Games",
                column: "Name",
                unique: true,
                filter: "lower(\"Name\") = lower(\"Name\")")
                .Annotation("Npgsql:IndexOperators", new[] { "text_pattern_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_Name",
                table: "Games");
        }
    }
}

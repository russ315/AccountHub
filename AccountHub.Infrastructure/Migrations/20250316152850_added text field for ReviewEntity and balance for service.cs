using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedtextfieldforReviewEntityandbalanceforservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceSchedules",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Stars",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceSchedules");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Stars",
                table: "Reviews");
        }
    }
}

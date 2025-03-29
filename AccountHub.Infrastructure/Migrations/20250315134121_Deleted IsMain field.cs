using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedIsMainfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "AccountImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "AccountImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDataTypeToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "GameVariants",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "GameVariants",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

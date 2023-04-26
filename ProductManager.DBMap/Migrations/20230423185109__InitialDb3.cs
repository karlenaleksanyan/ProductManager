using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.DBMap.Migrations
{
    /// <inheritdoc />
    public partial class _InitialDb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBasket",
                table: "Products",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBasket",
                table: "Products");
        }
    }
}

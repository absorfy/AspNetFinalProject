using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetFinalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class ListAndCardOrderIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Lists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Cards");
        }
    }
}

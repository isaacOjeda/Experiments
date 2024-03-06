using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modular.eShop.Catalogs.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Model_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                schema: "Catalog",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                schema: "Catalog",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                schema: "Catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                schema: "Catalog",
                table: "Products");
        }
    }
}

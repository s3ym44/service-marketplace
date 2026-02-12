using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class RecipeHierarchyAndConsumption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename Category → MainCategory (preserves data)
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "PackageItems",
                newName: "MainCategory");

            migrationBuilder.AlterColumn<string>(
                name: "MainCategory",
                table: "PackageItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ItemType",
                table: "PackageItems",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ConsumptionFormula",
                table: "PackageItems",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "PackageItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumptionFormula",
                table: "PackageItems");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "PackageItems");

            migrationBuilder.AlterColumn<string>(
                name: "ItemType",
                table: "PackageItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MainCategory",
                table: "PackageItems",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.RenameColumn(
                name: "MainCategory",
                table: "PackageItems",
                newName: "Category");
        }
    }
}

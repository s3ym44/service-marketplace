using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class MakeFirstNameLastNameNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_MainCategories_MainCategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_RecipeTemplates_RecipeTemplateId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeTemplateId",
                table: "Listings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MainCategoryId",
                table: "Listings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_MainCategories_MainCategoryId",
                table: "Listings",
                column: "MainCategoryId",
                principalTable: "MainCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_RecipeTemplates_RecipeTemplateId",
                table: "Listings",
                column: "RecipeTemplateId",
                principalTable: "RecipeTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_MainCategories_MainCategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_RecipeTemplates_RecipeTemplateId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeTemplateId",
                table: "Listings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainCategoryId",
                table: "Listings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_MainCategories_MainCategoryId",
                table: "Listings",
                column: "MainCategoryId",
                principalTable: "MainCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_RecipeTemplates_RecipeTemplateId",
                table: "Listings",
                column: "RecipeTemplateId",
                principalTable: "RecipeTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

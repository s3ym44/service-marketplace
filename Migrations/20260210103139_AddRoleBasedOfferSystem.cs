using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleBasedOfferSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Offers",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "ServiceTemplateItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfferType",
                table: "Offers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RenovationType",
                table: "Listings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceTemplateId",
                table: "Listings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessLicense",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UserId",
                table: "Offers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ServiceTemplateId",
                table: "Listings",
                column: "ServiceTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ServiceTemplates_ServiceTemplateId",
                table: "Listings",
                column: "ServiceTemplateId",
                principalTable: "ServiceTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_UserId",
                table: "Offers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ServiceTemplates_ServiceTemplateId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_UserId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_UserId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Listings_ServiceTemplateId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "ServiceTemplateItems");

            migrationBuilder.DropColumn(
                name: "OfferType",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "RenovationType",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ServiceTemplateId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "BusinessLicense",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Offers",
                newName: "SupplierId");
        }
    }
}

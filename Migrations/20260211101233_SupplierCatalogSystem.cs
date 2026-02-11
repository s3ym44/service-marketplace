using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class SupplierCatalogSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServicePackageId1",
                table: "Listings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupplierProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<string>(type: "text", nullable: false),
                    PackageItemId = table.Column<int>(type: "integer", nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ModelCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    InStock = table.Column<bool>(type: "boolean", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierProducts_AspNetUsers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierProducts_PackageItems_PackageItemId",
                        column: x => x.PackageItemId,
                        principalTable: "PackageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ServicePackageId1",
                table: "Listings",
                column: "ServicePackageId1");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_Category",
                table: "SupplierProducts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_PackageItemId",
                table: "SupplierProducts",
                column: "PackageItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_SupplierId_IsActive",
                table: "SupplierProducts",
                columns: new[] { "SupplierId", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ServicePackages_ServicePackageId1",
                table: "Listings",
                column: "ServicePackageId1",
                principalTable: "ServicePackages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ServicePackages_ServicePackageId1",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "SupplierProducts");

            migrationBuilder.DropIndex(
                name: "IX_Listings_ServicePackageId1",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ServicePackageId1",
                table: "Listings");
        }
    }
}

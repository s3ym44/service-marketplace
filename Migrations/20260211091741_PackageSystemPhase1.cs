using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class PackageSystemPhase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalculatedMetrics",
                table: "Listings",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "Listings",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicePackageId",
                table: "Listings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServicePackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    MainCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ThumbnailImage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackages_MainCategories_MainCategoryId",
                        column: x => x.MainCategoryId,
                        principalTable: "MainCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackageItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServicePackageId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ItemType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageItems_ServicePackages_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferLineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    PackageItemId = table.Column<int>(type: "integer", nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferLineItems_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferLineItems_PackageItems_PackageItemId",
                        column: x => x.PackageItemId,
                        principalTable: "PackageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ServicePackageId",
                table: "Listings",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferLineItems_OfferId",
                table: "OfferLineItems",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferLineItems_PackageItemId",
                table: "OfferLineItems",
                column: "PackageItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageItems_ServicePackageId",
                table: "PackageItems",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_MainCategoryId",
                table: "ServicePackages",
                column: "MainCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ServicePackages_ServicePackageId",
                table: "Listings",
                column: "ServicePackageId",
                principalTable: "ServicePackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ServicePackages_ServicePackageId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "OfferLineItems");

            migrationBuilder.DropTable(
                name: "PackageItems");

            migrationBuilder.DropTable(
                name: "ServicePackages");

            migrationBuilder.DropIndex(
                name: "IX_Listings_ServicePackageId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CalculatedMetrics",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ServicePackageId",
                table: "Listings");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminPriceReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RegionModifier = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPriceReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListingCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingId = table.Column<int>(type: "int", nullable: false),
                    RequiredPaint = table.Column<double>(type: "float", nullable: false),
                    RequiredPrimer = table.Column<double>(type: "float", nullable: false),
                    RequiredCeramic = table.Column<double>(type: "float", nullable: false),
                    EstimatedMaterialMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedMaterialMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedLaborMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedLaborMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedDaysMin = table.Column<int>(type: "int", nullable: false),
                    EstimatedDaysMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaborCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LaborCostType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialCostTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedDays = table.Column<int>(type: "int", nullable: false),
                    WarrantyMonths = table.Column<int>(type: "int", nullable: false),
                    MaterialSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalServicesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminPriceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferMaterials_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdminPriceReferences",
                columns: new[] { "Id", "BasePrice", "Brand", "Category", "IsActive", "MaterialName", "Quality", "Quantity", "RegionModifier", "Unit" },
                values: new object[,]
                {
                    { 1, 850m, "Fawori", "Boya", true, "İç Cephe Duvar Boyası", "Standart", 15m, 1.0, "Litre" },
                    { 2, 1200m, "Marshall", "Boya", true, "İç Cephe Duvar Boyası", "Premium", 15m, 1.0, "Litre" },
                    { 3, 450m, "Marshall", "Boya", true, "Astar", "Standart", 15m, 1.0, "Litre" },
                    { 4, 180m, "Ege Seramik", "Seramik", true, "Yer Seramiği 60x60", "Premium", 1m, 1.0, "m²" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferMaterials_OfferId",
                table: "OfferMaterials",
                column: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminPriceReferences");

            migrationBuilder.DropTable(
                name: "ListingCalculations");

            migrationBuilder.DropTable(
                name: "OfferMaterials");

            migrationBuilder.DropTable(
                name: "Offers");
        }
    }
}

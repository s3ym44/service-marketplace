using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogBasedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainCategoryId",
                table: "Listings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeTemplateId",
                table: "Listings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MainCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MainCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TotalItems = table.Column<int>(type: "integer", nullable: false),
                    EstimatedBudgetMin = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    EstimatedBudgetMax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeTemplates_MainCategories_MainCategoryId",
                        column: x => x.MainCategoryId,
                        principalTable: "MainCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeTemplateId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DefaultQuantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeItems_RecipeTemplates_RecipeTemplateId",
                        column: x => x.RecipeTemplateId,
                        principalTable: "RecipeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LaborCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RecipeItemId = table.Column<int>(type: "integer", nullable: false),
                    LaborType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FixedPricePerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    EstimatedDuration = table.Column<int>(type: "integer", nullable: false),
                    WarrantyMonths = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborCatalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaborCatalogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LaborCatalogs_RecipeItems_RecipeItemId",
                        column: x => x.RecipeItemId,
                        principalTable: "RecipeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RecipeItemId = table.Column<int>(type: "integer", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FixedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SKU = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierCatalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierCatalogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierCatalogs_RecipeItems_RecipeItemId",
                        column: x => x.RecipeItemId,
                        principalTable: "RecipeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    RecipeItemId = table.Column<int>(type: "integer", nullable: false),
                    SupplierCatalogId = table.Column<int>(type: "integer", nullable: true),
                    LaborCatalogId = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsManualPrice = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferItems_LaborCatalogs_LaborCatalogId",
                        column: x => x.LaborCatalogId,
                        principalTable: "LaborCatalogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfferItems_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferItems_RecipeItems_RecipeItemId",
                        column: x => x.RecipeItemId,
                        principalTable: "RecipeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferItems_SupplierCatalogs_SupplierCatalogId",
                        column: x => x.SupplierCatalogId,
                        principalTable: "SupplierCatalogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_MainCategoryId",
                table: "Listings",
                column: "MainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_RecipeTemplateId",
                table: "Listings",
                column: "RecipeTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LaborCatalogs_RecipeItemId",
                table: "LaborCatalogs",
                column: "RecipeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LaborCatalogs_UserId",
                table: "LaborCatalogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferItems_LaborCatalogId",
                table: "OfferItems",
                column: "LaborCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferItems_OfferId",
                table: "OfferItems",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferItems_RecipeItemId",
                table: "OfferItems",
                column: "RecipeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferItems_SupplierCatalogId",
                table: "OfferItems",
                column: "SupplierCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_RecipeTemplateId",
                table: "RecipeItems",
                column: "RecipeTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTemplates_MainCategoryId",
                table: "RecipeTemplates",
                column: "MainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierCatalogs_RecipeItemId",
                table: "SupplierCatalogs",
                column: "RecipeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierCatalogs_UserId",
                table: "SupplierCatalogs",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_MainCategories_MainCategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_RecipeTemplates_RecipeTemplateId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "OfferItems");

            migrationBuilder.DropTable(
                name: "LaborCatalogs");

            migrationBuilder.DropTable(
                name: "SupplierCatalogs");

            migrationBuilder.DropTable(
                name: "RecipeItems");

            migrationBuilder.DropTable(
                name: "RecipeTemplates");

            migrationBuilder.DropTable(
                name: "MainCategories");

            migrationBuilder.DropIndex(
                name: "IX_Listings_MainCategoryId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_RecipeTemplateId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "RecipeTemplateId",
                table: "Listings");
        }
    }
}

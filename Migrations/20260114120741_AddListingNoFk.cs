using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddListingNoFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "Offers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    CeilingHeight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedMaterialMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedMaterialMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedLaborMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedLaborMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedDaysMin = table.Column<int>(type: "int", nullable: false),
                    EstimatedDaysMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ListingId",
                table: "Offers",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserId",
                table: "Listings",
                column: "UserId");

            // NULL existing ListingIds to avoid FK conflict with orphan data
            migrationBuilder.Sql("UPDATE [Offers] SET [ListingId] = NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Listings_ListingId",
                table: "Offers",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Listings_ListingId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ListingId",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

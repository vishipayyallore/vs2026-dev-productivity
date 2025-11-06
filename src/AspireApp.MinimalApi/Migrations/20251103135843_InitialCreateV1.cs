using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspireApp.MinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HurricaneAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    WindSpeedMph = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ExpectedLandfall = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HurricaneAlerts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HurricaneAlerts",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ExpectedLandfall", "IsActive", "Location", "Name", "Severity", "UpdatedAt", "WindSpeedMph" },
                values: new object[,]
                {
                    { 1, 4, new DateTime(2024, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc), "Major hurricane with extremely dangerous winds. Prepare for life-threatening storm surge and destructive winds.", new DateTime(2024, 10, 15, 6, 0, 0, 0, DateTimeKind.Utc), true, "Gulf of Mexico, approaching Florida West Coast", "Hurricane Milton", 4, new DateTime(2024, 10, 14, 12, 0, 0, 0, DateTimeKind.Utc), 150.0 },
                    { 2, 1, new DateTime(2024, 10, 13, 8, 0, 0, 0, DateTimeKind.Utc), "Tropical storm conditions expected. Monitor for potential strengthening.", null, true, "Atlantic Ocean, moving northwest", "Tropical Storm Nadine", 2, new DateTime(2024, 10, 14, 8, 0, 0, 0, DateTimeKind.Utc), 85.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HurricaneAlerts_Category",
                table: "HurricaneAlerts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_HurricaneAlerts_IsActive",
                table: "HurricaneAlerts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_HurricaneAlerts_Name",
                table: "HurricaneAlerts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_HurricaneAlerts_Severity",
                table: "HurricaneAlerts",
                column: "Severity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HurricaneAlerts");
        }
    }
}

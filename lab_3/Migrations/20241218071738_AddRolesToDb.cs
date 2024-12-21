using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lab_3.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "36cb7a96-a976-4e50-af2b-37b15a3708a4", null, "manager", "manager" },
                    { "9c900da7-5586-44f8-848d-ed766fc6fc7c", null, "stationBoss", "stationBoss" },
                    { "d4a40918-0d71-4c01-b582-d380d1696322", null, "passenger", "passenger" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36cb7a96-a976-4e50-af2b-37b15a3708a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c900da7-5586-44f8-848d-ed766fc6fc7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4a40918-0d71-4c01-b582-d380d1696322");
        }
    }
}

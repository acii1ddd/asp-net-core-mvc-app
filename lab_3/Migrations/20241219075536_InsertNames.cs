using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lab_3.Migrations
{
    /// <inheritdoc />
    public partial class InsertNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "046ca7d3-dbc7-497f-a5d7-afe1da9c952e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b26c763-f5e7-4a66-92e3-d639d4772421");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7fcfd1d-8373-42cc-85c5-a1a7ab9cc73e");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e0b9c8b4-64e8-4738-89bd-f8675ec21097", null, "passenger", null },
                    { "f0ea9eeb-d045-47e4-9a7a-f20758271764", null, "stationBoss", null },
                    { "fcbac49d-3771-4c90-8317-4467d4807eeb", null, "manager", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0b9c8b4-64e8-4738-89bd-f8675ec21097");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0ea9eeb-d045-47e4-9a7a-f20758271764");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcbac49d-3771-4c90-8317-4467d4807eeb");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "046ca7d3-dbc7-497f-a5d7-afe1da9c952e", null, "stationBoss", "stationBoss" },
                    { "1b26c763-f5e7-4a66-92e3-d639d4772421", null, "manager", "manager" },
                    { "c7fcfd1d-8373-42cc-85c5-a1a7ab9cc73e", null, "passenger", "passenger" }
                });
        }
    }
}

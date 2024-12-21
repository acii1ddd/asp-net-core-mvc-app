using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lab_3.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLogPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "login",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "password",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "login",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}

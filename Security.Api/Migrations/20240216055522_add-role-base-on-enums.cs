using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Security.Api.Migrations
{
    /// <inheritdoc />
    public partial class addrolebaseonenums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2bf3411e-695b-4764-8705-8c7caf004469", null, "Admin", "ADMIN" },
                    { "bfdb2bc5-9edb-4b9c-a5c0-0e1194bd6679", null, "Member", "MEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2bf3411e-695b-4764-8705-8c7caf004469");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfdb2bc5-9edb-4b9c-a5c0-0e1194bd6679");
        }
    }
}

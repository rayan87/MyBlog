using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Migrations
{
    public partial class InitialCreateRefresh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61225c4c-ccc4-40e4-a1c9-44ff0224a256");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b34badd-05bf-4580-ba08-cf9b107440b7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c209d9d-0d49-4634-988c-1c7ce8dbb84e", "cba57e41-1af7-4ddd-b840-9680e63b7970", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d6aedf85-8ee7-44f3-9796-de8fa38dce3f", "9f888f24-45e2-4c8e-b0e1-7ab415413e25", "Author", "AUTHOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c209d9d-0d49-4634-988c-1c7ce8dbb84e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6aedf85-8ee7-44f3-9796-de8fa38dce3f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "61225c4c-ccc4-40e4-a1c9-44ff0224a256", "f204607c-84f0-452a-a967-1cdef0fd4599", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7b34badd-05bf-4580-ba08-cf9b107440b7", "501275a4-bf15-4979-b867-543621134bd4", "Author", null });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBookAPI.Migrations
{
    public partial class country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RefTerm",
                keyColumn: "Id",
                keyValue: new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f"),
                column: "description",
                value: "type of address#");

            migrationBuilder.InsertData(
                table: "RefTerm",
                columns: new[] { "Id", "description", "key" },
                values: new object[] { new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b"), "USA_TYPE", "USA" });

            migrationBuilder.InsertData(
                table: "RefTerm",
                columns: new[] { "Id", "description", "key" },
                values: new object[] { new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"), "INDIA_TYPE", "INDIA" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RefTerm",
                keyColumn: "Id",
                keyValue: new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b"));

            migrationBuilder.DeleteData(
                table: "RefTerm",
                keyColumn: "Id",
                keyValue: new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"));

            migrationBuilder.UpdateData(
                table: "RefTerm",
                keyColumn: "Id",
                keyValue: new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f"),
                column: "description",
                value: "type of address");
        }
    }
}

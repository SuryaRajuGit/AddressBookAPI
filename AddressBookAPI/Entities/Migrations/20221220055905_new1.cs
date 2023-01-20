using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBookAPI.Migrations
{
    public partial class new1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Email",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("0a2991e5-f5ea-4f36-8e75-5157440b4760"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("1a433bb5-263f-4df8-bec5-8318cb56755e"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("28f501c3-f8ae-48fe-bb35-e25ecaa56f22"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("50373bbd-5846-40cf-bd60-021de6f919a5"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("5554537c-5721-468d-a3e5-be5234778bf1"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("8beb045a-988a-4199-a0e4-0673b9d60e34"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("8ffc2abc-8abb-442e-85dd-bfa1fab18d85"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("ad1304c1-270a-481a-a99c-b1781e2ce33c"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("bf325767-e54a-47df-b733-cf7f65e64dee"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("edaa1847-9bcc-407b-894c-d7cb3caa615a"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("fb9646a4-3c58-43dd-a919-8dc457559422"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("b28f8b13-fab0-42cf-846d-225da6057a5a"));

            migrationBuilder.InsertData(
                table: "SetRefTerm",
                columns: new[] { "Id", "RefSetId", "RefTermId" },
                values: new object[,]
                {
                    { new Guid("cec21589-eb64-42a9-979a-1b603c771d1b"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("c326884e-e132-43b6-9a7c-3c05bdbc750f"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("17d60ec1-722c-4063-83f6-595cd3a75247"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("507a6c20-1a4a-425e-bafc-2ba01c939446"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("9fa96c0b-d4ba-4509-82a3-9b964058c241"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("35ffaf42-d1fc-43c1-bd16-651cb70f5142"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("f9e010d3-4bb4-42da-80d6-a5ff46ad569f"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("34a7e8e6-4dd5-4571-b4f8-8408d16df47e"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("7a63a47d-bcb8-43d5-a2d7-1c92430f6aad"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("01ca30af-24c0-4cba-892a-ebce093251ff"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2") },
                    { new Guid("a987948c-22f0-48ee-9893-5a5da65d4dd7"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b") }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("0fe5e908-cbd9-4c92-8445-32af3b939917"), "surya", "raju" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "RefTermId", "StateName", "UserId", "Zipcode" },
                values: new object[] { 1, "vizag", new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"), "s-1", "s2", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), "AndhraPradesh", new Guid("0fe5e908-cbd9-4c92-8445-32af3b939917"), "531116" });

            migrationBuilder.InsertData(
                table: "Email",
                columns: new[] { "Id", "EmailAddress", "RefTermId", "UserId" },
                values: new object[] { 1, "psuryaraju5@gmail.com", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), new Guid("0fe5e908-cbd9-4c92-8445-32af3b939917") });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "RefTermId", "UserId" },
                values: new object[] { 1, "8142255769", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), new Guid("0fe5e908-cbd9-4c92-8445-32af3b939917") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Email",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Phone",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("01ca30af-24c0-4cba-892a-ebce093251ff"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("17d60ec1-722c-4063-83f6-595cd3a75247"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("34a7e8e6-4dd5-4571-b4f8-8408d16df47e"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("35ffaf42-d1fc-43c1-bd16-651cb70f5142"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("507a6c20-1a4a-425e-bafc-2ba01c939446"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("7a63a47d-bcb8-43d5-a2d7-1c92430f6aad"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("9fa96c0b-d4ba-4509-82a3-9b964058c241"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("a987948c-22f0-48ee-9893-5a5da65d4dd7"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("c326884e-e132-43b6-9a7c-3c05bdbc750f"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("cec21589-eb64-42a9-979a-1b603c771d1b"));

            migrationBuilder.DeleteData(
                table: "SetRefTerm",
                keyColumn: "Id",
                keyValue: new Guid("f9e010d3-4bb4-42da-80d6-a5ff46ad569f"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("0fe5e908-cbd9-4c92-8445-32af3b939917"));

            migrationBuilder.InsertData(
                table: "SetRefTerm",
                columns: new[] { "Id", "RefSetId", "RefTermId" },
                values: new object[,]
                {
                    { new Guid("fb9646a4-3c58-43dd-a919-8dc457559422"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("50373bbd-5846-40cf-bd60-021de6f919a5"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("edaa1847-9bcc-407b-894c-d7cb3caa615a"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("28f501c3-f8ae-48fe-bb35-e25ecaa56f22"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("8beb045a-988a-4199-a0e4-0673b9d60e34"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("5554537c-5721-468d-a3e5-be5234778bf1"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("ad1304c1-270a-481a-a99c-b1781e2ce33c"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("bf325767-e54a-47df-b733-cf7f65e64dee"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("0a2991e5-f5ea-4f36-8e75-5157440b4760"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("8ffc2abc-8abb-442e-85dd-bfa1fab18d85"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2") },
                    { new Guid("1a433bb5-263f-4df8-bec5-8318cb56755e"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b") }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("b28f8b13-fab0-42cf-846d-225da6057a5a"), "surya", "raju" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "Country", "Line1", "Line2", "RefTermId", "StateName", "UserId", "Zipcode" },
                values: new object[] { 1, "vizag", new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"), "s-1", "s2", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), "AndhraPradesh", new Guid("b28f8b13-fab0-42cf-846d-225da6057a5a"), "531116" });

            migrationBuilder.InsertData(
                table: "Email",
                columns: new[] { "Id", "EmailAddress", "RefTermId", "UserId" },
                values: new object[] { 1, "psuryaraju5@gmail.com", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), new Guid("b28f8b13-fab0-42cf-846d-225da6057a5a") });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "PhoneNumber", "RefTermId", "UserId" },
                values: new object[] { 1, "8142255769", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), new Guid("b28f8b13-fab0-42cf-846d-225da6057a5a") });
        }
    }
}

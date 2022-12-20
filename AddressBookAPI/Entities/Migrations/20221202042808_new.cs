using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBookAPI.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefTerm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefTerm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetRefTerm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RefSetId = table.Column<Guid>(nullable: false),
                    RefTermId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetRefTerm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetRefTerm_RefSet_RefSetId",
                        column: x => x.RefSetId,
                        principalTable: "RefSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetRefTerm_RefTerm_RefTermId",
                        column: x => x.RefTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true),
                    Country = table.Column<Guid>(nullable: false),
                    RefTermId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_RefTerm_RefTermId",
                        column: x => x.RefTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Address_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Field = table.Column<byte[]>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetDTO_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    RefTermId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Email_RefTerm_RefTermId",
                        column: x => x.RefTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Email_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    RefTermId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phone_RefTerm_RefTermId",
                        column: x => x.RefTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Phone_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RefSet",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[,]
                {
                    { new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), "email", "EMAIL_ADDRESS_TYPE" },
                    { new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), "address ", "ADDRESS_TYPE" },
                    { new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), "country ", "COUNTRY" },
                    { new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), "phone", "PHONE_NUMBER_TYPE" }
                });

            migrationBuilder.InsertData(
                table: "RefTerm",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[,]
                {
                    { new Guid("12cf7780-9096-4855-a049-40476cead362"), "work type", "WORK" },
                    { new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), "personal type", "PERSONAL" },
                    { new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f"), "alternate  type", "ALTERNATE" },
                    { new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"), "INDIA_TYPE", "INDIA" },
                    { new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b"), "USA_TYPE", "USA" }
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

            migrationBuilder.InsertData(
                table: "SetRefTerm",
                columns: new[] { "Id", "RefSetId", "RefTermId" },
                values: new object[,]
                {
                    { new Guid("50373bbd-5846-40cf-bd60-021de6f919a5"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("8beb045a-988a-4199-a0e4-0673b9d60e34"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("bf325767-e54a-47df-b733-cf7f65e64dee"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("fb9646a4-3c58-43dd-a919-8dc457559422"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("28f501c3-f8ae-48fe-bb35-e25ecaa56f22"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("ad1304c1-270a-481a-a99c-b1781e2ce33c"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("edaa1847-9bcc-407b-894c-d7cb3caa615a"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("5554537c-5721-468d-a3e5-be5234778bf1"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("0a2991e5-f5ea-4f36-8e75-5157440b4760"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("8ffc2abc-8abb-442e-85dd-bfa1fab18d85"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2") },
                    { new Guid("1a433bb5-263f-4df8-bec5-8318cb56755e"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_RefTermId",
                table: "Address",
                column: "RefTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDTO_UserId",
                table: "AssetDTO",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_RefTermId",
                table: "Email",
                column: "RefTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_UserId",
                table: "Email",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_RefTermId",
                table: "Phone",
                column: "RefTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_UserId",
                table: "Phone",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRefTerm_RefSetId",
                table: "SetRefTerm",
                column: "RefSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRefTerm_RefTermId",
                table: "SetRefTerm",
                column: "RefTermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AssetDTO");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "SetRefTerm");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "RefSet");

            migrationBuilder.DropTable(
                name: "RefTerm");
        }
    }
}

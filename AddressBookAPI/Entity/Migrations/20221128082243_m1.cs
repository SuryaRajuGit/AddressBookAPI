using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBookAPI.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
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
                    key = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
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
                    key = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
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
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true)
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
                    refSetId = table.Column<Guid>(nullable: false),
                    refTermId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetRefTerm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetRefTerm_RefSet_refSetId",
                        column: x => x.refSetId,
                        principalTable: "RefSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetRefTerm_RefTerm_refTermId",
                        column: x => x.refTermId,
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
                    line1 = table.Column<string>(nullable: true),
                    line2 = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    zipCode = table.Column<string>(nullable: true),
                    stateName = table.Column<string>(nullable: true),
                    country = table.Column<Guid>(nullable: false),
                    refTermId = table.Column<Guid>(nullable: false),
                    userId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_RefTerm_refTermId",
                        column: x => x.refTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Address_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    field = table.Column<byte[]>(nullable: true),
                    userId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetDTO_User_userId",
                        column: x => x.userId,
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
                    emailAddress = table.Column<string>(nullable: true),
                    userId = table.Column<Guid>(nullable: false),
                    refTermId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Email_RefTerm_refTermId",
                        column: x => x.refTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Email_User_userId",
                        column: x => x.userId,
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
                    phoneNumber = table.Column<string>(nullable: true),
                    userId = table.Column<Guid>(nullable: false),
                    refTermId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phone_RefTerm_refTermId",
                        column: x => x.refTermId,
                        principalTable: "RefTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Phone_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RefSet",
                columns: new[] { "Id", "description", "key" },
                values: new object[,]
                {
                    { new Guid("7294ac28-c285-4476-89e9-0215d0cb96cd"), "india ", "INDIA" },
                    { new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), "email", "EMAIL_ADDRESS_TYPE" },
                    { new Guid("4adab962-e8c7-489d-b9eb-2d76c8cc30a2"), "usa", "UNITED_STATES" },
                    { new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), "address ", "ADDRESS_TYPE" },
                    { new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), "country ", "COUNTRY" },
                    { new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), "phone", "PHONE_NUMBER_TYPE" }
                });

            migrationBuilder.InsertData(
                table: "RefTerm",
                columns: new[] { "Id", "description", "key" },
                values: new object[,]
                {
                    { new Guid("12cf7780-9096-4855-a049-40476cead362"), "work type", "WORK" },
                    { new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), "personal type", "PERSONAL" },
                    { new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f"), "alternate  type", "ALTERNATE" },
                    { new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"), "INDIA_TYPE", "INDIA" },
                    { new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b"), "USA_TYPE", "USA" },
                    { new Guid("05e92a12-1241-4a96-92d9-0206e500efee"), "country", "COUNTRY" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "firstName", "lastName" },
                values: new object[] { new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4"), "surya", "raju" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "city", "country", "line1", "line2", "refTermId", "stateName", "userId", "zipCode" },
                values: new object[] { 1, "vizag", new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"), "s-1", "s2", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), "AndhraPradesh", new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4"), "531116" });

            migrationBuilder.InsertData(
                table: "Email",
                columns: new[] { "Id", "emailAddress", "refTermId", "userId" },
                values: new object[] { 1, "psuryaraju5@gmail.com", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4") });

            migrationBuilder.InsertData(
                table: "Phone",
                columns: new[] { "Id", "phoneNumber", "refTermId", "userId" },
                values: new object[] { 1, "8142255769", new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4") });

            migrationBuilder.InsertData(
                table: "SetRefTerm",
                columns: new[] { "Id", "refSetId", "refTermId" },
                values: new object[,]
                {
                    { new Guid("1202ff32-8983-4ef5-ab36-5151fbe5620b"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("3fd0f674-e20e-4cd2-8512-8600c3af40ff"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("d5cf481f-7d61-46e3-83b1-c36f1c419b2e"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("12cf7780-9096-4855-a049-40476cead362") },
                    { new Guid("46f05308-b5df-4ad5-a94b-2f9ba0aaa2db"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("221a524f-15eb-421c-8b96-383a4bfa1e46"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("4d4e3d74-b45f-40ef-94ba-7d9d50b0fcf3"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce") },
                    { new Guid("b3e4c0da-3621-43f7-9549-84cdfe91b349"), new Guid("b4005322-979c-4df5-96b2-16b2f6101006"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("0050c6c6-6ff7-486d-a2d4-75b7ebfaca0a"), new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("4ee96531-4dda-4d5b-9cb2-965bec063e72"), new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"), new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f") },
                    { new Guid("3580a95e-01a9-4298-93ec-57b57c155196"), new Guid("7294ac28-c285-4476-89e9-0215d0cb96cd"), new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2") },
                    { new Guid("d9428266-7e3a-4b58-9b8e-6724e5a00ee1"), new Guid("4adab962-e8c7-489d-b9eb-2d76c8cc30a2"), new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b") },
                    { new Guid("ecb24a03-2530-4c54-9f17-1e22900bd44d"), new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"), new Guid("05e92a12-1241-4a96-92d9-0206e500efee") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_refTermId",
                table: "Address",
                column: "refTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_userId",
                table: "Address",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDTO_userId",
                table: "AssetDTO",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_refTermId",
                table: "Email",
                column: "refTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_userId",
                table: "Email",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_refTermId",
                table: "Phone",
                column: "refTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_userId",
                table: "Phone",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRefTerm_refSetId",
                table: "SetRefTerm",
                column: "refSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRefTerm_refTermId",
                table: "SetRefTerm",
                column: "refTermId");
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

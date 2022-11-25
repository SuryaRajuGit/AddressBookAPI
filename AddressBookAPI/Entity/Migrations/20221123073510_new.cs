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
                    user_name = table.Column<string>(nullable: true),
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
                    first_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true)
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
                    state_name = table.Column<string>(nullable: true),
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
                    email_address = table.Column<string>(nullable: true),
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
                    phone_number = table.Column<string>(nullable: true),
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
                table: "RefTerm",
                columns: new[] { "Id", "description", "key" },
                values: new object[] { new Guid("12cf7780-9096-4855-a049-40476cead362"), "type of phone number", "WORK" });

            migrationBuilder.InsertData(
                table: "RefTerm",
                columns: new[] { "Id", "description", "key" },
                values: new object[] { new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"), "type of email ", "PERSONAL" });

            migrationBuilder.InsertData(
                table: "RefTerm",
                columns: new[] { "Id", "description", "key" },
                values: new object[] { new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f"), "type of address", "ALTERNATE" });

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

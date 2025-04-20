using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductInformationApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManufacturerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("5309b913-7cb5-48b4-b32d-2188da5ef21a"), "Apple Inc." },
                    { new Guid("c0db3ed1-80dc-491f-812a-84bdd749ba03"), "Sony Corporation" },
                    { new Guid("de29706e-27e0-4765-aeea-835cc605b4f3"), "Dell Technologies" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ManufacturerId", "Name" },
                values: new object[,]
                {
                    { new Guid("0d71b234-d672-4e8b-a7f5-13d55cd3b21e"), new Guid("de29706e-27e0-4765-aeea-835cc605b4f3"), "Alienware Aurora R13" },
                    { new Guid("4f3c2bd3-d55f-4d76-b31c-d49e99d6c721"), new Guid("5309b913-7cb5-48b4-b32d-2188da5ef21a"), "iPhone 15 Pro" },
                    { new Guid("5c79e189-2e8c-414e-bdf1-d3b3de2bb226"), new Guid("c0db3ed1-80dc-491f-812a-84bdd749ba03"), "Sony WH-1000XM5" },
                    { new Guid("b62b5be6-d52b-4e70-bc3d-20d4a8e3e8dc"), new Guid("5309b913-7cb5-48b4-b32d-2188da5ef21a"), "MacBook Air M3" },
                    { new Guid("e9171371-3aeb-4420-8ba0-91ec3b54193b"), new Guid("c0db3ed1-80dc-491f-812a-84bdd749ba03"), "PlayStation 5" },
                    { new Guid("fa3c2285-74f3-4c71-a2d3-3c8b80829ac7"), new Guid("de29706e-27e0-4765-aeea-835cc605b4f3"), "XPS 13" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufacturerId",
                table: "Products",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}

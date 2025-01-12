﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AllupNew.Migrations
{
    public partial class CreatedProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    DiscoutnPrice = table.Column<decimal>(type: "money", nullable: false),
                    ExTax = table.Column<decimal>(type: "money", nullable: false),
                    Seria = table.Column<string>(maxLength: 4, nullable: false),
                    Code = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

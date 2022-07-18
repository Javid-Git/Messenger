using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllupNew.Migrations
{
    public partial class UpdatedAppUsersTabelAddedConnectionIdAndConnectedAtColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConnectedAt",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "AspNetUsers",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "AspNetUsers");
        }
    }
}

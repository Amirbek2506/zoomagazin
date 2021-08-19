using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ZooMag.Migrations
{
    public partial class eddNewModelstoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalServs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServName = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ContentText = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalServs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsBanner = table.Column<int>(type: "integer", nullable: false),
                    AdditionalServId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServImages_AdditionalServs_AdditionalServId",
                        column: x => x.AdditionalServId,
                        principalTable: "AdditionalServs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "40cc5356-c615-4b70-b85b-6614e2baa71c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "63361cbe-d779-456d-afc1-303d891d11f1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a48ac866-db7e-42a8-865d-7ae2a7762cfb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "357ee2ce-f3fb-4c3c-a808-0a9194cf83ee", "AQAAAAEAACcQAAAAEF/ODW1fdzzRCiwSW0mTU+APG0qJUeLtq/tzjf1rgsSnxA1hWNwg2EadQ+pitldRSg==" });

            migrationBuilder.CreateIndex(
                name: "IX_ServImages_AdditionalServId",
                table: "ServImages",
                column: "AdditionalServId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServImages");

            migrationBuilder.DropTable(
                name: "AdditionalServs");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9f6d2846-9165-4ede-8ea9-255aeeb9f5d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4c5e75f4-18f0-48bf-bd7b-e32ece3bb27a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "aff7d911-7eff-4659-aea4-479323b53a15");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b6fb1942-6d7c-4f52-9415-6049936b8c71", "AQAAAAEAACcQAAAAED8Rcak0xZwaLORi8nWUffEdOrWmXFpJ3wK57dNR7ecAcfx+oWICLow//6YkkKvFzA==" });
        }
    }
}

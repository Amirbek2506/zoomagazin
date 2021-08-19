using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooMag.Migrations
{
    public partial class editServImageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanner",
                table: "ServImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsBannerImage",
                table: "ServImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e7ba4d0c-d91a-479f-99b2-953d8dc4f399");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "198246dd-73dc-447c-89dc-13d1ea977c3e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "fac513ee-6f50-4a74-9bc6-4d7d475f5349");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5bbc29f8-449e-43fb-9f80-f5af3f27ca17", "AQAAAAEAACcQAAAAED51ASjaKsfGxZrt+WrJvuHHL147Jk4d/BRteCmtFuXj5KUx9a7gK8KKzvNitglFDA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBannerImage",
                table: "ServImages");

            migrationBuilder.AddColumn<int>(
                name: "IsBanner",
                table: "ServImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}

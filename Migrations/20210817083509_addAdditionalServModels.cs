using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooMag.Migrations
{
    public partial class addAdditionalServModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "25f261fa-6b90-4671-9241-9f2ed3ff7ed7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "cf32a991-55c1-40fd-9d68-f05a9915c5ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5f79fab2-6f96-48c0-98f7-0b656d52bbb7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "65598075-6110-49ca-9d27-8b23639992d1", "AQAAAAEAACcQAAAAEBn1Qc0j+Hf8+70rSvvBC8UjjVnER0Sg2eSWiqplr2Jzj19BNkg0mgSQD0ineBP1Yg==" });
        }
    }
}

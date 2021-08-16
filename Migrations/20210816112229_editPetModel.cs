using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooMag.Migrations
{
    public partial class editPetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "Pets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Inoculated",
                table: "Pets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Inoculated",
                table: "Pets");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d0d4e215-f506-4f2e-bd39-ffb1ffe232aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9c1c3e32-c01e-4b3f-a365-97dd16e66554");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "ff7b68f3-92e4-4825-9bfe-a663f8ecf884");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "505b7e5b-0659-44c9-bee4-ad989c14186d", "AQAAAAEAACcQAAAAEOAMtkQ7MSUkeCS/CK+oyBwpFdQAV1d2qPJSWsF137MW+76h+nh2cgBMYzjAa80xgQ==" });
        }
    }
}

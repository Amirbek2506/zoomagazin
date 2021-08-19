using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ZooMag.Migrations
{
    public partial class editPerGaleyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetGaleries");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Pets");

            migrationBuilder.AddColumn<int>(
                name: "MainImageId",
                table: "Pets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PetImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PetId = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetImages_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PetImages_PetId",
                table: "PetImages",
                column: "PetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetImages");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Pets",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PetGaleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: true),
                    PetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetGaleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetGaleries_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "606a84a8-d730-4d77-8660-d2e04b2f33be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e670f9eb-8a6a-4746-910b-a0ce14fd95b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5db55a2a-4560-4940-a7f6-0d953d480d0f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "148b245e-2cda-4b4c-8d9d-31de473d0881", "AQAAAAEAACcQAAAAEBWhZOgGu+7t0W3BzS/VTxszIfIsBLhqIKR++oAustRh/FXoX5MKrKg6Ff0O2Crung==" });

            migrationBuilder.CreateIndex(
                name: "IX_PetGaleries_PetId",
                table: "PetGaleries",
                column: "PetId");
        }
    }
}

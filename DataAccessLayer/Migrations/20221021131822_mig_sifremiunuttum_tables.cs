using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class mig_sifremiunuttum_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SifremiUnuttums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RandomKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SifremiUnuttums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SifremiUnuttums_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SifremiUnuttumMailTuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SifremiUnuttumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SifremiUnuttumMailTuts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SifremiUnuttumMailTuts_SifremiUnuttums_SifremiUnuttumId",
                        column: x => x.SifremiUnuttumId,
                        principalTable: "SifremiUnuttums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SifremiUnuttumMailTuts_SifremiUnuttumId",
                table: "SifremiUnuttumMailTuts",
                column: "SifremiUnuttumId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SifremiUnuttums_AppUserId",
                table: "SifremiUnuttums",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SifremiUnuttumMailTuts");

            migrationBuilder.DropTable(
                name: "SifremiUnuttums");
        }
    }
}

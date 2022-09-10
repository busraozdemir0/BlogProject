using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class mig_formfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WriterImage",
                table: "Writers",
                newName: "WriterImageYol");

            migrationBuilder.RenameColumn(
                name: "BlogImage",
                table: "Blogs",
                newName: "BlogImageYol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WriterImageYol",
                table: "Writers",
                newName: "WriterImage");

            migrationBuilder.RenameColumn(
                name: "BlogImageYol",
                table: "Blogs",
                newName: "BlogImage");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BrightIdeas.Migrations
{
    public partial class BI03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Ideas",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Ideas",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}

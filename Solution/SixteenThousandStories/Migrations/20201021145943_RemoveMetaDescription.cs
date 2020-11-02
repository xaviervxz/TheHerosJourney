using Microsoft.EntityFrameworkCore.Migrations;

namespace SixteenThousandStories.Migrations
{
    public partial class RemoveMetaDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meta_Description",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "Meta_Description",
                table: "Outro");

            migrationBuilder.DropColumn(
                name: "Meta_Description",
                table: "Choice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meta_Description",
                table: "Scenario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Meta_Description",
                table: "Outro",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Meta_Description",
                table: "Choice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

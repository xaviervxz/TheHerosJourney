using Microsoft.EntityFrameworkCore.Migrations;

namespace SixteenThousandStories.Migrations
{
    public partial class RemoveOrderingInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Terrain");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Pronoun");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Morale");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Morale");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Mood");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Mood");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "JourneyStage");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Worth",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Difficulty");

            migrationBuilder.DropColumn(
                name: "Ordering_Int",
                table: "Archetype");

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Terrain",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Status",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Skill",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Pronoun",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Morale",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Mood",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "JourneyStage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Difficulty",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Argb",
                table: "Archetype",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Terrain");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Pronoun");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Morale");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Mood");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "JourneyStage");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Difficulty");

            migrationBuilder.DropColumn(
                name: "Argb",
                table: "Archetype");

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Terrain",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Skill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Skill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Pronoun",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Morale",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Morale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Mood",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Mood",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "JourneyStage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Worth",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Difficulty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordering_Int",
                table: "Archetype",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

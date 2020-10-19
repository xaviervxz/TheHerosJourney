using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SixteenThousandStories.Migrations
{
    public partial class HierarchyFillin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Location");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Created_ByID",
                table: "Location",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Archetype",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false),
                    Voice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archetype", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Archetype_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Difficulty",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulty", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Difficulty_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JourneyStage",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyStage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JourneyStage_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mood",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mood", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Mood_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Morale",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Morale", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Morale_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pronoun",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Object = table.Column<string>(nullable: true),
                    Adj_Possessive = table.Column<string>(nullable: true),
                    Pro_Possessive = table.Column<string>(nullable: true),
                    Reflexive = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pronoun", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pronoun_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Skill_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Status_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Terrain",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Ordering_Int = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terrain", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Terrain_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Outro",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Message_Template = table.Column<string>(nullable: true),
                    Meta_Description = table.Column<string>(nullable: true),
                    MoodID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outro", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Outro_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Outro_Mood_MoodID",
                        column: x => x.MoodID,
                        principalTable: "Mood",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scenario",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Message_Template = table.Column<string>(nullable: true),
                    Meta_Description = table.Column<string>(nullable: true),
                    MoodID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenario", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Scenario_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scenario_Mood_MoodID",
                        column: x => x.MoodID,
                        principalTable: "Mood",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scene",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    LocationID = table.Column<int>(nullable: true),
                    MoodID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scene", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Scene_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scene_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scene_Mood_MoodID",
                        column: x => x.MoodID,
                        principalTable: "Mood",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayCharacter",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    MoraleID = table.Column<int>(nullable: true),
                    Current_LocationID = table.Column<int>(nullable: true),
                    StageID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayCharacter", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlayCharacter_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayCharacter_Location_Current_LocationID",
                        column: x => x.Current_LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayCharacter_Morale_MoraleID",
                        column: x => x.MoraleID,
                        principalTable: "Morale",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayCharacter_JourneyStage_StageID",
                        column: x => x.StageID,
                        principalTable: "JourneyStage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ability",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Skill_BaseID = table.Column<int>(nullable: true),
                    Finding_DifficultyID = table.Column<int>(nullable: true),
                    Useing_DifficultyID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ability", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ability_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ability_Difficulty_Finding_DifficultyID",
                        column: x => x.Finding_DifficultyID,
                        principalTable: "Difficulty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ability_Skill_Skill_BaseID",
                        column: x => x.Skill_BaseID,
                        principalTable: "Skill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ability_Difficulty_Useing_DifficultyID",
                        column: x => x.Useing_DifficultyID,
                        principalTable: "Difficulty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Skill_BaseID = table.Column<int>(nullable: true),
                    Finding_DifficultyID = table.Column<int>(nullable: true),
                    Useing_DifficultyID = table.Column<int>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Worth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Item_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Difficulty_Finding_DifficultyID",
                        column: x => x.Finding_DifficultyID,
                        principalTable: "Difficulty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Skill_Skill_BaseID",
                        column: x => x.Skill_BaseID,
                        principalTable: "Skill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Difficulty_Useing_DifficultyID",
                        column: x => x.Useing_DifficultyID,
                        principalTable: "Difficulty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NPC",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Skill_BaseID = table.Column<int>(nullable: true),
                    Finding_DifficultyID = table.Column<int>(nullable: true),
                    Useing_DifficultyID = table.Column<int>(nullable: true),
                    ArchetypeID = table.Column<int>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Subj_Pronoun = table.Column<string>(nullable: true),
                    Obj_Pronoun = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPC", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NPC_Archetype_ArchetypeID",
                        column: x => x.ArchetypeID,
                        principalTable: "Archetype",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NPC_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NPC_Difficulty_Finding_DifficultyID",
                        column: x => x.Finding_DifficultyID,
                        principalTable: "Difficulty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NPC_Skill_Skill_BaseID",
                        column: x => x.Skill_BaseID,
                        principalTable: "Skill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NPC_Difficulty_Useing_DifficultyID",
                        column: x => x.Useing_DifficultyID,
                        principalTable: "Difficulty",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Choice",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Created_ByID = table.Column<int>(nullable: true),
                    Message_Template = table.Column<string>(nullable: true),
                    Meta_Description = table.Column<string>(nullable: true),
                    MoodID = table.Column<int>(nullable: true),
                    ScenarioID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Choice_User_Created_ByID",
                        column: x => x.Created_ByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Choice_Mood_MoodID",
                        column: x => x.MoodID,
                        principalTable: "Mood",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Choice_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_Created_ByID",
                table: "Location",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Ability_Created_ByID",
                table: "Ability",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Ability_Finding_DifficultyID",
                table: "Ability",
                column: "Finding_DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_Ability_Skill_BaseID",
                table: "Ability",
                column: "Skill_BaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Ability_Useing_DifficultyID",
                table: "Ability",
                column: "Useing_DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_Archetype_Created_ByID",
                table: "Archetype",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_Created_ByID",
                table: "Choice",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_MoodID",
                table: "Choice",
                column: "MoodID");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_ScenarioID",
                table: "Choice",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Difficulty_Created_ByID",
                table: "Difficulty",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Created_ByID",
                table: "Item",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Finding_DifficultyID",
                table: "Item",
                column: "Finding_DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Skill_BaseID",
                table: "Item",
                column: "Skill_BaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Useing_DifficultyID",
                table: "Item",
                column: "Useing_DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyStage_Created_ByID",
                table: "JourneyStage",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Mood_Created_ByID",
                table: "Mood",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Morale_Created_ByID",
                table: "Morale",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_ArchetypeID",
                table: "NPC",
                column: "ArchetypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_Created_ByID",
                table: "NPC",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_Finding_DifficultyID",
                table: "NPC",
                column: "Finding_DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_Skill_BaseID",
                table: "NPC",
                column: "Skill_BaseID");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_Useing_DifficultyID",
                table: "NPC",
                column: "Useing_DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_Outro_Created_ByID",
                table: "Outro",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Outro_MoodID",
                table: "Outro",
                column: "MoodID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayCharacter_Created_ByID",
                table: "PlayCharacter",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayCharacter_Current_LocationID",
                table: "PlayCharacter",
                column: "Current_LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayCharacter_MoraleID",
                table: "PlayCharacter",
                column: "MoraleID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayCharacter_StageID",
                table: "PlayCharacter",
                column: "StageID");

            migrationBuilder.CreateIndex(
                name: "IX_Pronoun_Created_ByID",
                table: "Pronoun",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_Created_ByID",
                table: "Scenario",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_MoodID",
                table: "Scenario",
                column: "MoodID");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_Created_ByID",
                table: "Scene",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_LocationID",
                table: "Scene",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_MoodID",
                table: "Scene",
                column: "MoodID");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_Created_ByID",
                table: "Skill",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Status_Created_ByID",
                table: "Status",
                column: "Created_ByID");

            migrationBuilder.CreateIndex(
                name: "IX_Terrain_Created_ByID",
                table: "Terrain",
                column: "Created_ByID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_Created_ByID",
                table: "Location",
                column: "Created_ByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_Created_ByID",
                table: "Location");

            migrationBuilder.DropTable(
                name: "Ability");

            migrationBuilder.DropTable(
                name: "Choice");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "NPC");

            migrationBuilder.DropTable(
                name: "Outro");

            migrationBuilder.DropTable(
                name: "PlayCharacter");

            migrationBuilder.DropTable(
                name: "Pronoun");

            migrationBuilder.DropTable(
                name: "Scene");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Terrain");

            migrationBuilder.DropTable(
                name: "Scenario");

            migrationBuilder.DropTable(
                name: "Archetype");

            migrationBuilder.DropTable(
                name: "Difficulty");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Morale");

            migrationBuilder.DropTable(
                name: "JourneyStage");

            migrationBuilder.DropTable(
                name: "Mood");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Location_Created_ByID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Created_ByID",
                table: "Location");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Location",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

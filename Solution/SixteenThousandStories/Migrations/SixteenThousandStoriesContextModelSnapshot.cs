﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SixteenThousandStories.Data;

namespace SixteenThousandStories.Migrations
{
    [DbContext(typeof(SixteenThousandStoriesContext))]
    partial class SixteenThousandStoriesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SixteenThousandStories.Models.Ability", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Finding_DifficultyID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Skill_BaseID")
                        .HasColumnType("int");

                    b.Property<int?>("Useing_DifficultyID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("Finding_DifficultyID");

                    b.HasIndex("Skill_BaseID");

                    b.HasIndex("Useing_DifficultyID");

                    b.ToTable("Ability");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Archetype", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.Property<string>("Voice")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Archetype");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Choice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message_Template")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Meta_Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MoodID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ScenarioID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("MoodID");

                    b.HasIndex("ScenarioID");

                    b.ToTable("Choice");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Difficulty", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Difficulty");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Item", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Finding_DifficultyID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("Skill_BaseID")
                        .HasColumnType("int");

                    b.Property<int?>("Useing_DifficultyID")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("Worth")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("Finding_DifficultyID");

                    b.HasIndex("Skill_BaseID");

                    b.HasIndex("Useing_DifficultyID");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.JourneyStage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("JourneyStage");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("ParentID");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Mood", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Mood");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Morale", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Morale");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.NPC", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("ArchetypeID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Finding_DifficultyID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Obj_Pronoun")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Skill_BaseID")
                        .HasColumnType("int");

                    b.Property<string>("Subj_Pronoun")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Useing_DifficultyID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ArchetypeID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("Finding_DifficultyID");

                    b.HasIndex("Skill_BaseID");

                    b.HasIndex("Useing_DifficultyID");

                    b.ToTable("NPC");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Outro", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message_Template")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Meta_Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MoodID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("MoodID");

                    b.ToTable("Outro");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.PlayCharacter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<int?>("Current_LocationID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MoraleID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StageID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("Current_LocationID");

                    b.HasIndex("MoraleID");

                    b.HasIndex("StageID");

                    b.ToTable("PlayCharacter");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Player", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Pronoun", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adj_Possessive")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Object")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.Property<string>("Pro_Possessive")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reflexive")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Pronoun");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Scenario", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message_Template")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Meta_Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MoodID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("MoodID");

                    b.ToTable("Scenario");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Scene", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LocationID")
                        .HasColumnType("int");

                    b.Property<int?>("MoodID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.HasIndex("LocationID");

                    b.HasIndex("MoodID");

                    b.ToTable("Scene");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Skill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Status", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Terrain", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Created_ByID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ordering_Int")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Created_ByID");

                    b.ToTable("Terrain");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Ability", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Difficulty", "Finding_Difficulty")
                        .WithMany()
                        .HasForeignKey("Finding_DifficultyID");

                    b.HasOne("SixteenThousandStories.Models.Skill", "Skill_Base")
                        .WithMany()
                        .HasForeignKey("Skill_BaseID");

                    b.HasOne("SixteenThousandStories.Models.Difficulty", "Useing_Difficulty")
                        .WithMany()
                        .HasForeignKey("Useing_DifficultyID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Archetype", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Choice", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Mood", "Mood")
                        .WithMany()
                        .HasForeignKey("MoodID");

                    b.HasOne("SixteenThousandStories.Models.Scenario", null)
                        .WithMany("Choices")
                        .HasForeignKey("ScenarioID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Difficulty", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Item", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Difficulty", "Finding_Difficulty")
                        .WithMany()
                        .HasForeignKey("Finding_DifficultyID");

                    b.HasOne("SixteenThousandStories.Models.Skill", "Skill_Base")
                        .WithMany()
                        .HasForeignKey("Skill_BaseID");

                    b.HasOne("SixteenThousandStories.Models.Difficulty", "Useing_Difficulty")
                        .WithMany()
                        .HasForeignKey("Useing_DifficultyID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.JourneyStage", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Location", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Location", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Mood", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Morale", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.NPC", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Archetype", "Archetype")
                        .WithMany()
                        .HasForeignKey("ArchetypeID");

                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Difficulty", "Finding_Difficulty")
                        .WithMany()
                        .HasForeignKey("Finding_DifficultyID");

                    b.HasOne("SixteenThousandStories.Models.Skill", "Skill_Base")
                        .WithMany()
                        .HasForeignKey("Skill_BaseID");

                    b.HasOne("SixteenThousandStories.Models.Difficulty", "Useing_Difficulty")
                        .WithMany()
                        .HasForeignKey("Useing_DifficultyID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Outro", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Mood", "Mood")
                        .WithMany()
                        .HasForeignKey("MoodID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.PlayCharacter", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Location", "Current_Location")
                        .WithMany()
                        .HasForeignKey("Current_LocationID");

                    b.HasOne("SixteenThousandStories.Models.Morale", "Morale")
                        .WithMany()
                        .HasForeignKey("MoraleID");

                    b.HasOne("SixteenThousandStories.Models.JourneyStage", "Stage")
                        .WithMany()
                        .HasForeignKey("StageID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Pronoun", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Scenario", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Mood", "Mood")
                        .WithMany()
                        .HasForeignKey("MoodID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Scene", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");

                    b.HasOne("SixteenThousandStories.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");

                    b.HasOne("SixteenThousandStories.Models.Mood", "Mood")
                        .WithMany()
                        .HasForeignKey("MoodID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Skill", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Status", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });

            modelBuilder.Entity("SixteenThousandStories.Models.Terrain", b =>
                {
                    b.HasOne("SixteenThousandStories.Models.Player", "Created_By")
                        .WithMany()
                        .HasForeignKey("Created_ByID");
                });
#pragma warning restore 612, 618
        }
    }
}

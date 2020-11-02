using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Data
{
    public class SixteenThousandStoriesContext : DbContext
    {
        public SixteenThousandStoriesContext (DbContextOptions<SixteenThousandStoriesContext> options)
            : base(options)
        {
        }

        public DbSet<SixteenThousandStories.Models.Location> Location { get; set; }

        public DbSet<SixteenThousandStories.Models.Pronoun> Pronoun { get; set; }

        public DbSet<SixteenThousandStories.Models.Player> User { get; set; }

        public DbSet<SixteenThousandStories.Models.Archetype> Archetype { get; set; }

        public DbSet<SixteenThousandStories.Models.Difficulty> Difficulty { get; set; }

        public DbSet<SixteenThousandStories.Models.JourneyStage> JourneyStage { get; set; }

        public DbSet<SixteenThousandStories.Models.Mood> Mood { get; set; }

        public DbSet<SixteenThousandStories.Models.Morale> Morale { get; set; }

        public DbSet<SixteenThousandStories.Models.Skill> Skill { get; set; }

        public DbSet<SixteenThousandStories.Models.Status> Status { get; set; }

        public DbSet<SixteenThousandStories.Models.Terrain> Terrain { get; set; }

        public DbSet<SixteenThousandStories.Models.Ability> Ability { get; set; }

        public DbSet<SixteenThousandStories.Models.Item> Item { get; set; }

        public DbSet<SixteenThousandStories.Models.PlayCharacter> PlayCharacter { get; set; }

        public DbSet<SixteenThousandStories.Models.NPC> NPC { get; set; }

        public DbSet<SixteenThousandStories.Models.Scenario> Scenario { get; set; }

        public DbSet<SixteenThousandStories.Models.Scene> Scene { get; set; }

        public DbSet<SixteenThousandStories.Models.Outro> Outro { get; set; }

        public DbSet<SixteenThousandStories.Models.Choice> Choice { get; set; }

        public DbSet<SixteenThousandStories.Models.SpecificTerrain> SpecificTerrain { get; set; }
    }
}

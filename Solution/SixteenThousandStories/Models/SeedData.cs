using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SixteenThousandStories.Data;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using TheHerosJourney.Functions;

namespace SixteenThousandStories.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SixteenThousandStoriesContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SixteenThousandStoriesContext>>()))
            {

                /*var characterDataStream = GetDataResourceStream("character_data.json");
                var locationDataStream = GetDataResourceStream("location_data.json");
                var scenesStream = GetDataResourceStream("scenes.csv");
                var adventuresStream = GetDataResourceStream("adventures.csv");
                using (FileStream defaultJSON = File.OpenRead("Data/location_data.json")){

                    LoadFromFile.ReadAllText(defaultJSON);
                    dynamic locations_json = JsonConvert. DeserializeObject(defaultJSON.);

                }*/



                Player seed = new Player
                {
                    Username = "SeededData",
                    Created = DateTime.Parse("1989-2-12"),
                };
                // Look for any locations.
                if (!context.User.Any())
                {
                    context.User.AddRange(
                        seed
                    );
                }

                // Look for any pronouns.
                if (!context.Pronoun.Any())
                {
                    context.Pronoun.AddRange(
                        new Pronoun
                        {
                            Name = "Male",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Default Masculine Gender Pronouns",
                            Subject = "He",
                            Object = "Him",
                            Adj_Possessive = "His",
                            Pro_Possessive = "His",
                            Reflexive = "Himself",
                            Created_By = seed
                        },

                        new Pronoun
                        {
                            Name = "Female",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Default Feminine Gender Pronouns",
                            Subject = "She",
                            Object = "Her",
                            Adj_Possessive = "Her",
                            Pro_Possessive = "Hers",
                            Reflexive = "Herself",
                        },

                        new Pronoun
                        {
                            Name = "Indistinct",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Default Non-Gendered Pronouns",
                            Subject = "They",
                            Object = "Them",
                            Adj_Possessive = "Their",
                            Pro_Possessive = "Theirs",
                            Reflexive = "Themself",
                        }
                    );
                }

                // Look for any locations.
                if (!context.Location.Any())
                {
                    context.Location.AddRange(
                        new Location
                        {
                            Name = "Balerno",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The City of a Thousand Deaths",
                            Created_By = seed,
                        }
                    );
                }

                // Look for any locations.
                if (!context.Difficulty.Any())
                {
                    context.Difficulty.AddRange(
                        new Difficulty
                        {
                            Name = "Inconvenient",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "All you need to do is decide that the task should be done, and it becomes so.",
                            Created_By = seed,
                        },
                        new Difficulty
                        {
                            Name = "Troublesome",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "If at first you don't succeed, you probably just need to put in a little more elbow grease.",
                            Created_By = seed,
                        },
                        new Difficulty
                        {
                            Name = "Challenging",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Some tasks require you to grow to meet them. This is one of those.",
                            Created_By = seed,
                        },
                        new Difficulty
                        {
                            Name = "Strenuous",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "An expert might devote a year of their life to this problem and still come away without a solution.",
                            Created_By = seed,
                        },
                        new Difficulty
                        {
                            Name = "Ambitious",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A plan is only as good as its actors, and a plan for this task must have redundancies built in.",
                            Created_By = seed,
                        },
                        new Difficulty
                        {
                            Name = "Heroic",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "After you have died, and the people you know have followed, and even everyone that they had known passes, this legend of this task will continue.",
                            Created_By = seed,
                        },
                        new Difficulty
                        {
                            Name = "Impossible",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "If you could pull heaven down to earth and break God on his throne, you could still not manage this task.",
                            Created_By = seed,
                        }
                    );
                }
                // Look for any locations.
                if (!context.Skill.Any())
                {
                    context.Skill.AddRange(
                        new Skill
                        {
                            Name = "Empathy",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A skill for understanding others, from helping conversation to detecting deception.",
                            Created_By = seed,
                            Color = Color.DarkViolet
                        },
                        new Skill
                        {
                            Name = "Cunning",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Sometimes the best path goes outside the rules.",
                            Created_By = seed,
                            Color = Color.DarkTurquoise
                        },
                        new Skill
                        {
                            Name = "Strength",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "For pushing through, especially through literal walls.",
                            Created_By = seed,
                            Color = Color.Red
                        },
                        new Skill
                        {
                            Name = "Agility",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Because a fast reaction often beats a strong hit.",
                            Created_By = seed,
                            Color = Color.Gold
                        },
                        new Skill
                        {
                            Name = "Knowledge",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Some people read books for distraction; others, to consume their meanings.",
                            Created_By = seed,
                            Color = Color.WhiteSmoke
                        },
                        new Skill
                        {
                            Name = "Luck",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Even a master needs things to go their way.",
                            Created_By = seed,
                            Color = Color.LawnGreen
                        }
                    ) ;
                }
                // Look for any locations.
                if (!context.Ability.Any())
                {
                    context.Ability.AddRange(
                        new Ability
                        {
                            Name = "Fireball",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A conjured ball of fire, useful for burning and pillaging",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(4),
                            Useing_Difficulty = context.Difficulty.Find(4),
                            Skill_Base = context.Skill.Where(s => s.Name == "Knowledge").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "Eye Contact & Nodding",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "It doesn't matter if you follow what they're saying, as long as they feel like they talked to you.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(1),
                            Useing_Difficulty = context.Difficulty.Find(1),
                            Skill_Base = context.Skill.Where(s => s.Name == "Empathy").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "Active Listening",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "You can pick up a lot from just asking questions.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(3),
                            Skill_Base = context.Skill.Where(s => s.Name == "Empathy").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "Long Jump",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "All you need to leap is a bit of confidence.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(1),
                            Useing_Difficulty = context.Difficulty.Find(2),
                            Skill_Base = context.Skill.Where(s => s.Name == "Strength").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "Pocket Sand",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "They call it a dirty trick, but dirt doesn't work nearly as well.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(1),
                            Useing_Difficulty = context.Difficulty.Find(2),
                            Skill_Base = context.Skill.Where(s => s.Name == "Cunning").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "See a Copper",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "You never know what that copper piece will do for you.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(1),
                            Useing_Difficulty = context.Difficulty.Find(1),
                            Skill_Base = context.Skill.Where(s => s.Name == "Luck").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "Parry",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A quick weapon strike to disrupt an opponent's attack.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(3),
                            Skill_Base = context.Skill.Where(s => s.Name == "Agility").FirstOrDefault()
                        },
                        new Ability
                        {
                            Name = "Dodge",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Why play tricks when you can roll out of the way?",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(2),
                            Skill_Base = context.Skill.Where(s => s.Name == "Agility").FirstOrDefault()
                        }
                    );
                }
                // Look for any locations.
                if (!context.Archetype.Any())
                {
                    context.Archetype.AddRange(
                        new Archetype
                        {
                            Name = "Mother",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "They don't ever want anyone to suffer, and see the best in everyone.",
                            Created_By = seed,
                            Voice = "Soft"
                        },
                        new Archetype
                        {
                            Name = "Mentor",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "They're not here to change the world- just to give other people the world-changing knowledge.",
                            Created_By = seed,
                            Voice = "Measured"
                        },
                        new Archetype
                        {
                            Name = "Child",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Unaware and out of their depth.",
                            Created_By = seed,
                            Voice = "Bouncey"
                        },
                        new Archetype
                        {
                            Name = "Cynic",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "They've seen the worst that humanity can do, and done some of it themselves.",
                            Created_By = seed,
                            Voice = "Disinterested"
                        },
                        new Archetype
                        {
                            Name = "Trickster",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Less interested in goals than living in the moment, especially if 'living in the moment' means making your life difficult.",
                            Created_By = seed,
                            Voice = "Smooth"
                        },
                        new Archetype
                        {
                            Name = "Hero",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A creature that knows exactly what it owes the world- whatever they give it.",
                            Created_By = seed,
                            Voice = "Proud"
                        }
                    );
                }

                // Look for any locations.
                if (!context.Item.Any())
                {
                    context.Item.AddRange(
                        new Item
                        {
                            Name = "Iron Sword",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Sometimes it's just nice to have some metal with an edge.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(2),
                            Skill_Base = context.Skill.Where(s => s.Name == "Strength").FirstOrDefault(),
                            Weight = 3
                        },
                        new Item
                        {
                            Name = "Wooden Shield",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Blocks anything from Snowballs to wolf teeth.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(1),
                            Skill_Base = context.Skill.Where(s => s.Name == "Strength").FirstOrDefault(),
                            Weight = 1
                        },
                        new Item
                        {
                            Name = "Whip",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "It's loud and it's painful, but mostly useful against animals.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(2),
                            Skill_Base = context.Skill.Where(s => s.Name == "Agility").FirstOrDefault(),
                            Weight = 1
                        },
                        new Item
                        {
                            Name = "Textbook",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "It's loud and it's painful, but mostly useful against animals.",
                            Created_By = seed,
                            Finding_Difficulty = context.Difficulty.Find(2),
                            Useing_Difficulty = context.Difficulty.Find(2),
                            Skill_Base = context.Skill.Where(s => s.Name == "Agility").FirstOrDefault(),
                            Weight = 1
                        }
                    );
                }
                // Look for any locations.
                if (!context.JourneyStage.Any())
                {
                    context.JourneyStage.AddRange(
                        new JourneyStage
                        {
                            Name = "Backstory",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Events that have happened before the hero sets off.",
                            Created_By = seed
                        },
                        new JourneyStage
                        {
                            Name = "Call to Adventure",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The hero begins in a situation of normality from which some information is received that acts as a call to head off into the unknown.",
                            Created_By = seed
                        },
                        new JourneyStage
                        {
                            Name = "Refusal of the Call",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Often when the call is given, the future hero first refuses to heed it. This may be from a sense of duty or obligation, fear, insecurity, a sense of inadequacy, or any of a range of reasons that work to hold the person in his current circumstances.",
                            Created_By = seed
                        }
                    );
                }

                // Look for any locations.
                if (!context.Mood.Any())
                {
                    context.Mood.AddRange(
                        new Mood
                        {
                            Name = "Despairing",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The darkest moment.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 1
                        },
                        new Mood
                        {
                            Name = "Depressed",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Hope is most important when is seems impossible.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 2
                        },
                        new Mood
                        {
                            Name = "Displeased",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A rock in your shoe isn't going to make your day better.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 3
                        },
                        new Mood
                        {
                            Name = "Calm",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The winds blow, but you feel calm.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 4
                        },
                        new Mood
                        {
                            Name = "Happy",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A compliment from a stranger.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 5
                        },
                        new Mood
                        {
                            Name = "Joyful",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Even the rain is beautiful.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 6
                        },
                        new Mood
                        {
                            Name = "Ecstatic",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Everything's coming up positive.",
                            Created_By = seed,
                            Color = Color.White,
                            ID = 7
                        }
                    );
                }

                // Look for any locations.
                if (!context.Morale.Any())
                {
                    context.Morale.AddRange(
                        new Morale
                        {
                            Name = "Despairing",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The darkest moment.",
                            Created_By = seed,
                            Color = Color.Black,
                            ID = 1
                        },
                        new Morale
                        {
                            Name = "Depressed",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Hope is most important when is seems impossible.",
                            Created_By = seed,
                            Color = Color.Red,
                            ID = 2
                        },
                        new Morale
                        {
                            Name = "Displeased",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A rock in your shoe isn't going to make your day better.",
                            Created_By = seed,
                            Color = Color.Orange,
                            ID = 3
                        },
                        new Morale
                        {
                            Name = "Calm",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The winds blow, but you feel calm.",
                            Created_By = seed,
                            Color = Color.Yellow,
                            ID = 4
                        },
                        new Morale
                        {
                            Name = "Happy",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A compliment from a stranger.",
                            Created_By = seed,
                            Color = Color.Green,
                            ID = 5
                        },
                        new Morale
                        {
                            Name = "Joyful",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Even the rain is beautiful.",
                            Created_By = seed,
                            Color = Color.Blue,
                            ID = 6
                        },
                        new Morale
                        {
                            Name = "Ecstatic",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Everything's coming up positive.",
                            Created_By = seed,
                            Color = Color.White,
                            ID = 7
                        }
                    );
                }

                // Look for any locations.
                if (!context.Status.Any())
                {
                    context.Status.AddRange(
                        new Status
                        {
                            Name = "Dead",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Dead doesn't always mean gone.",
                            Created_By = seed
                        },
                        new Status
                        {
                            Name = "Introduced",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "The description has been given before.",
                            Created_By = seed,
                        },
                        new Status
                        {
                            Name = "Lost",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Also missing. Some things just can't be found without a lot of work.",
                            Created_By = seed,
                        },
                        new Status
                        {
                            Name = "With",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "This entity is traveling with the player character.",
                            Created_By = seed,
                        },
                        new Status
                        {
                            Name = "Killed",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "I think maybe some of these need double targets",
                            Created_By = seed,
                        },
                        new Status
                        {
                            Name = "Destroyed",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "Dead may not always be gone, but you burn a corpse and it won't have a chance to come back.",
                            Created_By = seed,
                        }
                    );
                }

                // Look for any locations.
                if (!context.Terrain.Any())
                {
                    context.Terrain.AddRange(
                        new Terrain
                        {
                            Name = "Village",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A small, quiet town. Think Cottagecore.",
                            Created_By = seed
                        },
                        new Terrain
                        {
                            Name = "Meadow",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A grassy, treeless space under tall trees.",
                            Created_By = seed
                        }
                    );
                }

                // Look for any locations.
                if (!context.Terrain.Any())
                {
                    context.Terrain.AddRange(
                        new Terrain
                        {
                            Name = "Village",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A small, quiet town. Think Cottagecore.",
                            Created_By = seed
                        },
                        new Terrain
                        {
                            Name = "Meadow",
                            Created_At = DateTime.Parse("1989-2-12"),
                            Description = "A grassy, treeless space under tall trees.",
                            Created_By = seed
                        }
                    );
                }







                context.SaveChanges();
            }
        }
    }
}

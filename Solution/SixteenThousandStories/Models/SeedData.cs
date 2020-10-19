using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SixteenThousandStories.Data;
using System;
using System.Linq;

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
                // Look for any pronouns.
                if (context.Pronoun.Any())
                {
                    return;   // DB has been seeded
                }

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
                
                // Look for any locations.
                if (context.Location.Any())
                {
                    return;   // DB has been seeded
                }

                context.Location.AddRange(
                    new Location
                    {
                        Name = "Balerno",
                        Created_At = DateTime.Parse("1989-2-12"),
                        Description = "The City of a Thousand Deaths",
                        //Created_By = ,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

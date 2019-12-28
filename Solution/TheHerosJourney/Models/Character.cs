using TheHerosJourney.Functions;
using System.Collections.Generic;

namespace TheHerosJourney.Models
{
    public class Character
    {
        public Character()
        {
            Sex = Pick.Random(new[] { Sex.Female, Sex.Male });
        }

        public Sex Sex { get; set; }

        public string SubPronoun => Sex == Sex.Female ? "she" : "he";
        
        public string ObjPronoun => Sex == Sex.Female ? "her" : "him";
        
        public string PossPronoun => Sex == Sex.Female ? "her" : "his";

        public string SexAge => Relationship == Relationship.Child ? (Sex == Sex.Female ? "girl" : "boy") : (Sex == Sex.Female ? "woman" : "man");

        public string Baron => Sex == Sex.Female ? "Baroness" : "Baron";

        public string Chief => Sex == Sex.Female ? "Chieftess" : "Chief";

        public string Name { get; set; }

        public Relationship Relationship { get; set; }

        public List<Item> Inventory { get; } = new List<Item>();

        public Town Hometown { get; set; }

        public Location CurrentLocation { get; set; }

        public Location Goal { get; set; }

#if DEBUG
        public override string ToString()
        {
            return Name;
        }
#endif
    }
    public enum Sex
    {
        Female,
        Male
    }

    public enum Relationship
    {
        Self,
        Stranger,
        Friend,
        Mentor,
        Antagonist,
        Child,
        Mysterious
    }

    public class Item
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
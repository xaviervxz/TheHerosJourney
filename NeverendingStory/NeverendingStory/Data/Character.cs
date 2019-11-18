using NeverendingStory.Functions;
using System.Collections.Generic;

namespace NeverendingStory.Data
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

        public string Name { get; set; }

        public Relationship Relationship { get; set; }

        public List<Item> Inventory { get; set; } = new List<Item>{new Item
        {
            Identifier = "clothes",
            Name = "Clothes",
            Description = "shirt, pants, shoes, and a cloak"
        }};

        public Location Hometown { get; set; }
    }
    public enum Sex
    {
        Female,
        Male
    }

    public enum Relationship
    {
        Self,
        BestFriend,
        Mentor,
        Antagonist,
        Child,
        Hermit
    }

    public class Item
    {
        public string Identifier { get; internal set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
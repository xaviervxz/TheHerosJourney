using NeverendingStory.Functions;
using System.Collections.Generic;

namespace NeverendingStory.Data
{
    public class Character
    {
        public Character()
        {
            SexEnum = Pick.Random(new[] { Sex.Female, Sex.Male });
        }

        internal Sex SexEnum { get; set; }

        public string SubPronoun => SexEnum == Sex.Female ? "she" : "he";
        
        public string ObjPronoun => SexEnum == Sex.Female ? "her" : "him";
        
        public string PossPronoun => SexEnum == Sex.Female ? "her" : "his";

        public string SexAge => SexEnum == Sex.Female ? "woman" : "man";

        public string Baron => SexEnum == Sex.Female ? "Baroness" : "Baron";

        public string Name { get; set; }

        public Relationship Relationship { get; set; }

        public List<Item> Inventory { get; set; } = new List<Item>{new Item
        {
            Identifier = "clothes",
            Name = "Clothes",
            Description = "shirt, pants, shoes, and a cloak"
        }};
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
        Antagonist
    }

    public class Item
    {
        public string Identifier { get; internal set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
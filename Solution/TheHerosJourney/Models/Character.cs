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

        public string SexAge => Age == Age.Child ? (Sex == Sex.Female ? "girl" : "boy") : (Sex == Sex.Female ? "woman" : "man");

        public string Baron => Sex == Sex.Female ? "Baroness" : "Baron";

        public string Chief => Sex == Sex.Female ? "Chieftess" : "Chief";

        public string Name { get; set; }

        public Relationship Relationship { get; set; } = Relationship.Friend;

        public Occupation Occupation { get; set; } = Occupation.Worker;

        public Age Age { get; set; } = Age.Adult;

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

    public enum Age
    {
        Adult,
        Child
    }

    public enum Relationship
    {
        Friend,
        Foe
    }

    public enum Occupation
    {
        Worker,
        Ranger,
        Noble,
        Spirit,
        Criminal,
        Soldier
    }

    public class Item
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
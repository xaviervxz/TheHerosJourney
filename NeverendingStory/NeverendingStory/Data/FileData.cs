using System.Collections.Generic;

namespace NeverendingStory.Data
{
    using PeopleNames = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;
    //using TownData = Dictionary<Biome, Dictionary<Sex, string[]>>;

    public class FileData
    {
        public PeopleNames PeopleNames { get; set; }

        //public TownData TownData { get; set; }

        public Scene[] Scenes { get; set; }
    }

    public enum PeopleNameOrigin
    {
        Archaic,
        Westron,
        Southron,
        Northish
    }
}

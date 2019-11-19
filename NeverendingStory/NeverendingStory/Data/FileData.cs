using System.Collections.Generic;

namespace NeverendingStory.Data
{
    using PeopleNames = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;

    public class FileData
    {
        public PeopleNames PeopleNames { get; set; }

        public LocationData LocationData { get; set; }

        public Scene[] Scenes { get; set; }
    }

    public enum PeopleNameOrigin
    {
        Archaic,
        Westron,
        Southron,
        Northish
    }

    public class LocationData
    {
        public LocationNames Names { get; set; }
    }

    public class LocationNames
    {
        public string[] Adjectives { get; set; }

        public string[] Nouns { get; set; }

        public Dictionary<LocationType, LocationTerrain> Terrain { get; set; }
    }

    public class LocationTerrain
    {
        public string[] SpecificTypes { get; set; }

        public string[] Formats { get; set; }
    }
}

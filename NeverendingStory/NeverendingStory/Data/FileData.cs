using System.Collections.Generic;

namespace NeverendingStory.Data
{
    using CharacterData = Dictionary<PeopleNameOrigin, Dictionary<Sex, string[]>>;

    public class FileData
    {
        public CharacterData CharacterData { get; set; }

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

        public MainGeologicalFeature[] MainGeologicalFeatures { get; set; }

        public Dictionary<Industry, IndustryData> Industries { get; set; }
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

    public class MainGeologicalFeature
    {
        public LocationType[] Types { get; set; }

        public string RelativePosition { get; set; }
    }
}

using System.Collections.Generic;

namespace TheHerosJourney.Models
{
    public class FileData
    {
        public Dictionary<Sex, string[]> CharacterData { get; set; }

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

        public TownTemplate[] Towns { get; set; }

        public Dictionary<string, MainFeature> MainFeatures { get; set; }

        public Dictionary<Industry, IndustryData> Industries { get; set; }
    }

    public class LocationNames
    {
        public string[] Adjectives { get; set; }

        public string[] Nouns { get; set; }

        public string[] TheNouns { get; set; }

        public Dictionary<LocationType, LocationTerrain> Terrain { get; set; }
    }

    public class TownTemplate
    {
        public string Name { get; set; }

        public string MainFeature { get; set; }

        public Industry Industry { get; set; }
    }

    public class LocationTerrain
    {
        public string[] SpecificTypes { get; set; }

        public string[] Formats { get; set; }

        public string DescType { get; set; }
    }

    public class MainFeature
    {
        public LocationType[] Types { get; set; }

        public string RelativePosition { get; set; }
    }
}

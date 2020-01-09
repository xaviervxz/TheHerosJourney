using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace TheHerosJourney.Models
{
    public class Adventure
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Conditions { get; set; }

        [Name("RequiredScenes")]
        public string RawRequiredScenes { get; set; }

        [Ignore] // CsvReader Attribute
        public string[] RequiredSceneIds { get; set; }
        
        [Name("Transitions")]
        public string RawTransitions { get; set; }

        [Ignore] // CsvReader Attribute
        public Dictionary<JourneyStage, string[]> Transitions { get; set; }

        [Ignore]
        public bool Done { get; set; }
    }
}

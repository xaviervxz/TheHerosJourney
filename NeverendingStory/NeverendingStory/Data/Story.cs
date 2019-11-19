using System;
using System.Collections.Generic;

namespace NeverendingStory.Data
{
    public class Story
    {
        // STAGE-TRACKING

        public JourneyStage CurrentStage { get; set; } = JourneyStage.CallToAdventure;

        public int CurrentStageNumber { get; set; } = 0;

        public string NextSceneIdentifier { get; set; } = null;


        // STORY DATA

        public Character You { get; set; }


        // ***********
        // CHARACTERS
        // ***********

        public List<Character> Characters { get; } = new List<Character>();

        /// <summary>
        /// Contains references to characters in the Characters list.
        /// </summary>
        public Dictionary<string, Character> NamedCharacters { get; } = new Dictionary<string, Character>();
        
        // ***********
        // LOCATIONS
        // ***********

        public List<Location> Locations { get; } = new List<Location>();

        public List<Tuple<string, string>> NearbyLocations { get; } = new List<Tuple<string, string>>();

        /// <summary>
        /// Contains references to characters in the Characters list.
        /// </summary>
        public Dictionary<string, Location> NamedLocations { get; } = new Dictionary<string, Location>();
    }
}
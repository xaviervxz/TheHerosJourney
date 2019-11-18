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

        public List<Character> Characters { get; } = new List<Character>();

        /// <summary>
        /// Contains references to characters in the Characters list.
        /// </summary>
        public Dictionary<string, Character> NamedCharacters { get; } = new Dictionary<string, Character>();
    }
}
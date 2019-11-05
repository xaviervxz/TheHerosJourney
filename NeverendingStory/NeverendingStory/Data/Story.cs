using System.Collections.Generic;

namespace NeverendingStory.Data
{
    public class Story
    {
        public JourneyStage CurrentStage { get; set; } = JourneyStage.CallToAdventure;

        public Character You { get; set; }

        public Dictionary<string, Character> NamedCharacters { get; } = new Dictionary<string, Character>();

        public List<Character> Characters { get; } = new List<Character>();

        public int CurrentStageNumber { get; set; } = 0;

        public string NextSceneIdentifier { get; set; } = null;
    }
}
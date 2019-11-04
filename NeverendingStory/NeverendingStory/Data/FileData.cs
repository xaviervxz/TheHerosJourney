using System.Collections.Generic;

namespace NeverendingStory.Data
{
    using Names = Dictionary<NameOrigin, Dictionary<Sex, string[]>>;

    public class FileData
    {
        public Names Names { get; set; }

        public Scene[] Scenes { get; set; }
    }

    public enum NameOrigin
    {
        Archaic,
        Westron,
        Southron,
        Northish
    }

    public class Scene
    {
        public JourneyStage Stage { get; set; }

        public string Message { get; set; }

        public string Choice1 { get; set; }

        public string Choice2 { get; set; }

        public string Outro1 { get; set; }

        public string Outro2 { get; set; }
    }
}

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
}

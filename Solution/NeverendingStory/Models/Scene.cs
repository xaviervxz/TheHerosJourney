using CsvHelper.Configuration.Attributes;

namespace NeverendingStory.Data
{
    public class Scene
    {
        public string Identifier { get; set; }

        public string Conditions { get; set; }

        [Ignore]
        public JourneyStage? Stage { get; set; }

        [Ignore]
        public bool IsSubStage { get; set; }

        public string Message { get; set; }

        public string Choice1 { get; set; }

        public string Choice2 { get; set; }

        public string Outro1 { get; set; }

        public string Outro2 { get; set; }

        [Ignore]
        public bool Done { get; set; }

#if DEBUG
        public override string ToString()
        {
            return Identifier;
        }
#endif
    }
}

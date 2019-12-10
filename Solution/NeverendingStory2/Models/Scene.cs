using CsvHelper.Configuration.Attributes;

namespace NeverendingStory.Models
{
    public class Scene
    {
        public string Identifier { get; set; }

        public string Conditions { get; set; }

        [Ignore] // - for CSV Helper package, or similar
        public JourneyStage? Stage { get; set; }

        [Ignore] // - for CSV Helper package, or similar
        public bool IsSubStage { get; set; }

        public string Message { get; set; }

        public string Choice1 { get; set; }

        public string Choice2 { get; set; }

        public string Outro1 { get; set; }

        public string Outro2 { get; set; }

        [Ignore] // - for CSV Helper package, or similar
        public bool Done { get; set; }

#if DEBUG
        public override string ToString()
        {
            return Identifier;
        }
#endif
    }
}

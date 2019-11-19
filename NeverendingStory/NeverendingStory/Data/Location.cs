namespace NeverendingStory.Data
{
    public class Location
    {
        public string Name { get; set; }

        public bool HasThe { get; set; }

        public string NameWithThe => HasThe ? "the " + Name : Name;

        public virtual LocationType Type { get; set; }

        public string SpecificType { get; set; }
    }

    public enum LocationType
    {
        Forest,
        Swamp,
        Lake,
        River,
        Bay,
        Town
    }
}

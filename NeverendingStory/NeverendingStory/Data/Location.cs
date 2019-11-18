namespace NeverendingStory.Data
{
    public class Location
    {
        public string Name { get; set; }

        public LocationType Type { get; set; }
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

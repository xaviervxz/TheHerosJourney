namespace NeverendingStory.Models
{
    public class Road : Location
    {
        public Location RunsThrough { get; set; }

#if DEBUG
        public override string ToString()
        {
            return Name;
        }
#endif
    }
}

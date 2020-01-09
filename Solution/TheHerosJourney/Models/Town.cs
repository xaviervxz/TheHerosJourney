namespace TheHerosJourney.Models
{
    public class Town : Location
    {
        public override LocationType Type => LocationType.Town;

        public Feature MainFeature { get; set; }

        public NearbyRegion? NearbyRegion { get; set; }

        public string MainIndustry { get; set; }

        public IndustryData MainIndustryData { get; set; }
    }

    public class Feature
    {
        public Location[] Locations { get; set; }

        public string RelativePosition { get; set; }
    }

    public class IndustryData
    {
        public string WorkGer { get; set; }

        public string Workplace { get; set; }
        
        public string GoodsGer { get; set; }

        public string Goods { get; set; }

        public string Purpose { get; set; }
        
        public string GoodDay { get; set; }

        public string GoodDayFinal { get; set; }
    }

    public enum NearbyRegion
    {
        NearMountains,
        NearForest,
        NearSwamp,
        InMountains,
        InForest,
        InSwamp,
        InPlains
    }
}

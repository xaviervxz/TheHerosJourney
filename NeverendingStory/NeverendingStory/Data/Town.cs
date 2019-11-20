namespace NeverendingStory.Data
{
    public class Town : Location
    {
        public override LocationType Type => LocationType.Town;

        public Feature MainFeature { get; set; }

        public Industry MainIndustry { get; set; }

        public IndustryData MainIndustryData { get; set; }

        public Location[] NearbyLocations { get; set; }
    }

    public class Feature
    {
        public Location[] Locations { get; set; }

        public string RelativePosition { get; set; }
    }

    public class IndustryData
    {
        public string Workplace { get; set; }

        public string WorkGer { get; set; }
        
        public string Goods { get; set; }

        public string Purpose { get; set; }
        
        public string GoodDay { get; set; }

        public string GoodDayFinal { get; set; }
    }

    public enum Industry
    {
        IronOre,
        CopperOre,
        Gold,
        Silver,
        Clay,
        Granite,
        Quartz,
        Salt,
        Peat,
        Coal,
        HardwoodLumber,
        Barley,
        Oat,
        Bean,
        Corn,
        NutsAndOlives,
        Rice,
        Wheat,
        PotatoesAndLeeks,
        SugarCane,
        Tobacco,
        Cotton,
        FruitTrees,
        CabbageAndBeets,
        Cattle,
        DairyCows,
        Sheep,
        Fishing
    }
}

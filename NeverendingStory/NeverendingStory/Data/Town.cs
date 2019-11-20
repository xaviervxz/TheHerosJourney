namespace NeverendingStory.Data
{
    public class Town : Location
    {
        public override LocationType Type => LocationType.Town;

        public GeologicalFeature MainGeologicalFeature { get; set; }


        public Industry MainIndustry { get; set; }

        public Location[] NearbyLocations { get; set; }
    }

    public class GeologicalFeature
    {
        public Location[] Locations { get; set; }

        public string RelativePosition { get; set; }
    }

    public enum Industry
    {
        IronOreMining,
        CopperOreMining,
        GoldMining,
        SilverMining,
        ClayMining,
        GraniteMining,
        QuartzMining,
        SaltMining,
        PeatMining,
        CoalMining,
        HardwoodLumber,
        BarleyFarming,
        OatFarming,
        BeanFarming,
        CornFarming,
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

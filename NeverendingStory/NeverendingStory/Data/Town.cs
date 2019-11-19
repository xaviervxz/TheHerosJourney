namespace NeverendingStory.Data
{
    public class Town : Location
    {
        public override LocationType Type => LocationType.Town;

        public Location MainGeologicalFeature { get; set; }

        public Industry MainIndustry { get; set; }

        public Location[] NearbyLocations { get; set; }
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

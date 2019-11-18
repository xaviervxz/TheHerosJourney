using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverendingStory.Data
{
    public class Town
    {
        public string Name { get; set; }

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
    }
}

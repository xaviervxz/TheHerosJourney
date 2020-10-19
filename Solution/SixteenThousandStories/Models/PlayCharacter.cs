using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class PlayCharacter : DB_Entity
    {
        public Morale Morale { get; set; }
        public Location Current_Location { get; set; }
        public JourneyStage Stage { get; set; }
    }
}

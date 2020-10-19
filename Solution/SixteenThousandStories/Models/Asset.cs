using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Asset : DB_Entity
    {
        public Skill Skill_Base { get; set; }
        public Difficulty Finding_Difficulty { get; set; }
        public Difficulty Useing_Difficulty { get; set; }

    }
}

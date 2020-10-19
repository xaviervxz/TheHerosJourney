using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class NPC : Asset
    {
        public Archetype Archetype { get; set; }
        public int Age { get; set; }
        public String Subj_Pronoun { get; set; }
        public String Obj_Pronoun { get; set; }

    }
}

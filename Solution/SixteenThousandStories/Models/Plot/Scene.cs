using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Scene : DB_Entity
    {
        public Location Location { get; set; }
        public Mood Mood { get; set; }
    }
}

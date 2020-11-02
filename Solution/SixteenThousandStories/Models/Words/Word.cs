using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Word : DB_Entity
    {
        // True = Positive, False = Negative, Null = Neutral
        public bool? Connotation { get; set; }
    }
}

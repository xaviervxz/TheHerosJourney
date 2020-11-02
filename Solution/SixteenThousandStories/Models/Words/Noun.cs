using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Noun : Word
    {
        public bool Proper { get; set; }
        public string Plural { get; set; }
        public string Possessive { get; set; }
    }
}

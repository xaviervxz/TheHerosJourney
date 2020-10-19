using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Pronoun : Category
    {
        public string Subject { get; set; }
        public string Object { get; set; }
        public string Adj_Possessive { get; set; }
        public string Pro_Possessive { get; set; }
        public string Reflexive { get; set; }

    }
}

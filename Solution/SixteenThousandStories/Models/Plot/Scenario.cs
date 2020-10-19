using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Scenario : Message
    {
        public Mood Mood { get; set; }

        public List<Choice> Choices { get; set; }
    }
}

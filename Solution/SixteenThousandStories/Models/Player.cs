using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Username { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
    }
}

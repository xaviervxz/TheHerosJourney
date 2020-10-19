using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Item : Asset
    {
        public int Weight { get; set; }
        public int Quantity { get; set; }
        public int Worth { get; set; }

    }
}

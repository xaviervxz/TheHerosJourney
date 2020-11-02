using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class ItemConnector : Connector
    {
        public new Item Target { get; set; }

        public int Quantity { get; set; }
    }
}

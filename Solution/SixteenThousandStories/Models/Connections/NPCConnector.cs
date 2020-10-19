using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class NPCConnector : Connector
    {
        public new NPC Target { get; set; }

    }
}

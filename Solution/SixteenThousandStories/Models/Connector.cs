using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Connector : DB_Entity
    {
        public PlayCharacter Player{ get; set; }
        public DB_Entity Target { get; set; }
        public Status Status { get; set; }

    }
}

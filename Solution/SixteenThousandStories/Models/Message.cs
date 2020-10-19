using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class Message : DB_Entity
    {
        public string Message_Template { get; set; }
        public string Meta_Description { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SixteenThousandStories.Models
{
    public class SpecificTerrain : Category
    {
        Terrain Terrain { get; set; }
    }
}

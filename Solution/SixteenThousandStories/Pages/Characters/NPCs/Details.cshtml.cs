﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixteenThousandStories.Data;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Pages.Characters.NPCs
{
    public class DetailsModel : PageModel
    {
        private readonly SixteenThousandStories.Data.SixteenThousandStoriesContext _context;

        public DetailsModel(SixteenThousandStories.Data.SixteenThousandStoriesContext context)
        {
            _context = context;
        }

        public NPC NPC { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NPC = await _context.NPC.FirstOrDefaultAsync(m => m.ID == id);

            if (NPC == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

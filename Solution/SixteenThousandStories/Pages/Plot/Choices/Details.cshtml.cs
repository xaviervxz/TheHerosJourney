﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixteenThousandStories.Data;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Pages.Plot.Choices
{
    public class DetailsModel : PageModel
    {
        private readonly SixteenThousandStories.Data.SixteenThousandStoriesContext _context;

        public DetailsModel(SixteenThousandStories.Data.SixteenThousandStoriesContext context)
        {
            _context = context;
        }

        public Choice Choice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Choice = await _context.Choice.FirstOrDefaultAsync(m => m.ID == id);

            if (Choice == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

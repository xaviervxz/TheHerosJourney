﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixteenThousandStories.Data;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Pages.Categories.Statuses
{
    public class DeleteModel : PageModel
    {
        private readonly SixteenThousandStories.Data.SixteenThousandStoriesContext _context;

        public DeleteModel(SixteenThousandStories.Data.SixteenThousandStoriesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Status Status { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Status = await _context.Status.FirstOrDefaultAsync(m => m.ID == id);

            if (Status == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Status = await _context.Status.FindAsync(id);

            if (Status != null)
            {
                _context.Status.Remove(Status);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

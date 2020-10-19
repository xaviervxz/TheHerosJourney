using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixteenThousandStories.Data;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Pages.Plot.Scenes
{
    public class IndexModel : PageModel
    {
        private readonly SixteenThousandStories.Data.SixteenThousandStoriesContext _context;

        public IndexModel(SixteenThousandStories.Data.SixteenThousandStoriesContext context)
        {
            _context = context;
        }

        public IList<Scene> Scene { get;set; }

        public async Task OnGetAsync()
        {
            Scene = await _context.Scene.ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixteenThousandStories.Data;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Pages.Plot.Choices
{
    public class IndexModel : PageModel
    {
        private readonly SixteenThousandStories.Data.SixteenThousandStoriesContext _context;

        public IndexModel(SixteenThousandStories.Data.SixteenThousandStoriesContext context)
        {
            _context = context;
        }

        public IList<Choice> Choice { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Moods { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChoiceMood { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> moodQuery = from m in _context.Mood
                                            orderby m.ID
                                            select m.Name;
            var choices = from m in _context.Choice
                          select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                choices = choices.Where(s => s.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ChoiceMood))
            {
                choices = choices.Where(x => x.Mood.Name == ChoiceMood);
            }

            Moods = new SelectList(await moodQuery.Distinct().ToListAsync());
            Choice = await choices.ToListAsync();
        }
    }
}

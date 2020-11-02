using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixteenThousandStories.Data;
using SixteenThousandStories.Models;

namespace SixteenThousandStories.Pages.Plot.Outros
{
    public class CreateModel : PageModel
    {
        private readonly SixteenThousandStories.Data.SixteenThousandStoriesContext _context;

        public CreateModel(SixteenThousandStories.Data.SixteenThousandStoriesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Outro Outro { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Outro.Add(Outro);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public String FillTemplate(String template)
        {
            String result = template;
            String replace_pattern = @"(?<connector>\w+):(?<status>[\w|]+):(?<target>[\w| ]+)";
            String matcher = @"\{(" + replace_pattern + @")(\|" + replace_pattern + @")*\}";


            MatchCollection replacables = Regex.Matches(template, matcher);
            foreach (Match match in replacables)
            {
                String connector = match.Groups["connector"].Value;
                String status = match.Groups["status"].Value;
                String target = match.Groups["target"].Value;



            }

            return result;
        }
        /*
        public Connector GetConnection(String Connector, String Status, String Target)
        {
            Connector connection;
            switch (Connector)
            {
                case "ability":
                    connection = from m in _context.AbilityConnector select m;

            }
        }
        */
    }
}

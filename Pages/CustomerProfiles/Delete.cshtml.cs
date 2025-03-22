using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Threading.Tasks;

namespace MyWebApp.Pages.CustomerProfiles
{
    public class DeleteModel : PageModel
    {
        private readonly MyWebApp.Data.ApplicationDbContext _context;

        public DeleteModel(MyWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerProfile CustomerProfile { get; set; } = default!;

        public string DisplayFirstName => CustomerProfile?.FirstName ?? "N/A";
        public string DisplayLastName => CustomerProfile?.LastName ?? "N/A";
        public string DisplayEmail => CustomerProfile?.Email ?? "N/A";
        public string DisplayPhoneNumber => CustomerProfile?.PhoneNumber ?? "N/A";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerprofile = await _context.CustomerProfiles.FirstOrDefaultAsync(m => m.Id == id);

            if (customerprofile == null)
            {
                return NotFound();
            }
            else 
            {
                CustomerProfile = customerprofile;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerprofile = await _context.CustomerProfiles.FindAsync(id);

            if (customerprofile != null)
            {
                CustomerProfile = customerprofile;
                _context.CustomerProfiles.Remove(CustomerProfile);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

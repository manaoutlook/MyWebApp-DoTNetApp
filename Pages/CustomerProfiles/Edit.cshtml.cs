using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Threading.Tasks;

namespace MyWebApp.Pages.CustomerProfiles
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerProfile? CustomerProfile { get; set; } // Allow nullable

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerProfile = await _context.CustomerProfiles.FindAsync(id);

            if (CustomerProfile == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (CustomerProfile == null)
            {
                Console.WriteLine("CustomerProfile is null");
                ModelState.AddModelError(string.Empty, "Customer profile data is missing");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            try
            {
                var customerProfileToUpdate = await _context.CustomerProfiles.FindAsync(CustomerProfile.Id);

                if (customerProfileToUpdate == null)
                {
                    ModelState.AddModelError(string.Empty, "Customer profile not found");
                    return Page();
                }

                customerProfileToUpdate.FirstName = CustomerProfile.FirstName;
                customerProfileToUpdate.LastName = CustomerProfile.LastName;
                customerProfileToUpdate.Email = CustomerProfile.Email;
                customerProfileToUpdate.PhoneNumber = CustomerProfile.PhoneNumber;

                await _context.SaveChangesAsync();
                return RedirectToPage("/CustomerProfiles/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        private bool CustomerProfileExists(int id)
        {
            return _context.CustomerProfiles.Any(e => e.Id == id);
        }
    }
}

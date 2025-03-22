using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApp.Pages.CustomerProfiles
{
public class IndexModel : PageModel
{
    private readonly MyWebApp.Data.ApplicationDbContext _context;
    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public IndexModel(MyWebApp.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public PaginatedList<CustomerProfile> CustomerProfiles { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? FilterBy { get; set; }

    public List<int> PageSizeOptions { get; } = new() { 10, 20, 50, 100 };

    public async Task OnGetAsync(int? pageIndex)
    {
        var customerProfiles = _context.CustomerProfiles.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                customerProfiles = customerProfiles.Where(c => 
                    EF.Functions.Like(c.LastName, $"%{SearchString}%") ||
                    EF.Functions.Like(c.FirstName, $"%{SearchString}%") ||
                    EF.Functions.Like(c.Email, $"%{SearchString}%"));
            }

        if (!string.IsNullOrEmpty(FilterBy))
        {
            switch (FilterBy)
            {
                case "email":
                    customerProfiles = customerProfiles.OrderBy(c => c.Email);
                    break;
                case "name":
                    customerProfiles = customerProfiles.OrderBy(c => c.LastName);
                    break;
                case "phone":
                    customerProfiles = customerProfiles.OrderBy(c => c.PhoneNumber);
                    break;
            }
        }
        else
        {
            customerProfiles = customerProfiles.OrderBy(c => c.LastName);
        }

        CustomerProfiles = await PaginatedList<CustomerProfile>.CreateAsync(
            customerProfiles.AsNoTracking(), pageIndex ?? 1, PageSize);
    }
}

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
}

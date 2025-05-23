using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using lab9.Data;
using lab9.Models;
using lab9.Helpers;
using System.Text;

namespace lab9.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Class NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new();

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public List<Class> FilteredClasses { get; set; } = new();
        public string LoggedInUsername { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAuthenticated()) return RedirectToPage("Login");

            LoggedInUsername = HttpContext.Session.GetString("username");

            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
                query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));

            TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)PageSize);

            FilteredClasses = await query
                .OrderBy(c => c.Id)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
{
    if (!IsAuthenticated()) return RedirectToPage("Login");

    ModelState.Remove("SearchName"); // çünkü sadece GET'te dolu olabilir

    if (!ModelState.IsValid)
    {
        foreach (var modelState in ModelState)
        {
            foreach (var error in modelState.Value.Errors)
            {
                Console.WriteLine($"Validation error on {modelState.Key}: {error.ErrorMessage}");
            }
        }

        await OnGetAsync();
        return Page();
    }

    _context.Classes.Add(NewClass);
    await _context.SaveChangesAsync();

    return RedirectToPage();
}


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!IsAuthenticated()) return RedirectToPage("Login");

            var classToRemove = await _context.Classes.FindAsync(id);
            if (classToRemove != null)
            {
                _context.Classes.Remove(classToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            if (!IsAuthenticated()) return RedirectToPage("Login");

            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
                query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));

            var exportData = await query.ToListAsync();
            string json = Utils.Instance.ExportToJson(exportData, SelectedColumns);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            return File(byteArray, "application/json", "exported_classes.json");
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString("token") == Request.Cookies["token"] &&
                   HttpContext.Session.GetString("username") == Request.Cookies["username"] &&
                   HttpContext.Session.Id == Request.Cookies["session_id"];
        }
    }
}

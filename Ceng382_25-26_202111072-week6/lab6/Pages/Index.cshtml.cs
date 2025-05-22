using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YourProjectNamespace.Models;
using System.Collections.Generic;
using System.Linq;

namespace YourProjectNamespace.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> ClassList = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public List<ClassInformationTable> FilteredClasses { get; set; }

        public void OnGet()
        {
            if (ClassList.Count < 100)
            {
                for (int i = 1; i <= 100; i++)
                {
                    ClassList.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = "Class " + i,
                        StudentCount = 20 + (i % 30),
                        Description = "Sample class description " + i
                    });
                }
            }

            var query = ClassList.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));
            }

            TotalPages = (int)System.Math.Ceiling(query.Count() / (double)PageSize);
            query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

            FilteredClasses = query
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                }).ToList();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            NewClass.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(NewClass);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var classToRemove = ClassList.FirstOrDefault(c => c.Id == id);
            if (classToRemove != null)
            {
                ClassList.Remove(classToRemove);
            }
            return RedirectToPage();
        }
    }
}

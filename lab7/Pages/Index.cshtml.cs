using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab7.Models;
using lab7.Helpers;
using System.Text;

namespace lab7.Pages
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

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new();

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public List<ClassInformationTable> FilteredClasses { get; set; } = new();

        public void OnGet()
        {
            // Sadece 1 kez veri üret (ilk çalıştırmada)
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
                query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));

            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);

            FilteredClasses = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
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
                ClassList.Remove(classToRemove);

            return RedirectToPage();
        }

        public IActionResult OnPostExport()
        {
            var query = ClassList.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
                query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));

            var exportData = query
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                }).ToList();

            string json = Utils.Instance.ExportToJson(exportData, SelectedColumns);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            return File(byteArray, "application/json", "exported_classes.json");
        }
    }
}

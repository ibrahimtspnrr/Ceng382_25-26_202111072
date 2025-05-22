using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using YourProjectNamespace.Models;

namespace YourProjectNamespace.Pages
{
    public class IndexModel : PageModel
    {
        public List<ClassInformationModel> ClassList { get; set; } = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        public void OnGet()
        {
            if (TempData["ClassList"] is string rawJson)
            {
                ClassList = System.Text.Json.JsonSerializer.Deserialize<List<ClassInformationModel>>(rawJson) ?? new();
            }
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            if (TempData["ClassList"] is string rawJson)
            {
                ClassList = System.Text.Json.JsonSerializer.Deserialize<List<ClassInformationModel>>(rawJson) ?? new();
            }

            ClassList.Add(NewClass);
            TempData["ClassList"] = System.Text.Json.JsonSerializer.Serialize(ClassList);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (TempData["ClassList"] is string rawJson)
            {
                ClassList = System.Text.Json.JsonSerializer.Deserialize<List<ClassInformationModel>>(rawJson) ?? new();
            }

            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
                TempData["ClassList"] = System.Text.Json.JsonSerializer.Serialize(ClassList);
            }

            return RedirectToPage();
        }
    }
}

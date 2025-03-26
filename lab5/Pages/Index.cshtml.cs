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
            if (TempData["ClassList"] is List<ClassInformationModel> storedList)
            {
                ClassList = storedList;
            }
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            if (TempData["ClassList"] is List<ClassInformationModel> storedList)
            {
                ClassList = storedList;
            }

            ClassList.Add(NewClass);
            TempData["ClassList"] = ClassList; // Listeyi TempData ile sakla
            return RedirectToPage(); // Sayfayı yenileyerek listeyi güncelle
        }

        public IActionResult OnPostDelete(int id)
        {
            if (TempData["ClassList"] is List<ClassInformationModel> storedList)
            {
                ClassList = storedList;
            }

            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
                TempData["ClassList"] = ClassList;
            }

            return RedirectToPage();
        }
    }
}

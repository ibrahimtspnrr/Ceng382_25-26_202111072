using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab8.Models;
using System.Text.Json;

namespace lab8.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; } = "";

        public IActionResult OnPost()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
            var json = System.IO.File.ReadAllText(path);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            var user = users?.FirstOrDefault(u => u.Username == Username && u.Password == Password && u.IsActive);

            if (user != null)
            {
                var token = Guid.NewGuid().ToString();

                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("token", token);
                HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

                Response.Cookies.Append("username", user.Username);
                Response.Cookies.Append("token", token);
                Response.Cookies.Append("session_id", HttpContext.Session.Id);

                return RedirectToPage("Index");
            }

            ErrorMessage = "Invalid credentials.";
            return Page();
        }
    }
}

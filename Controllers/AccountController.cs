using DayRun.Web.Services;
using DayRun.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DayRun.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _api;
        public AccountController(ApiService api) { _api = api; }

        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var response = await _api.PostAsync("/api/auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonDocument.Parse(content).RootElement;
                var user = result.GetProperty("user");

                HttpContext.Session.SetString("UserId", user.GetProperty("userId").GetInt32().ToString());
                HttpContext.Session.SetString("Role", user.GetProperty("role").GetString()!);
                HttpContext.Session.SetString("Name", user.GetProperty("firstName").GetString()!);

                return user.GetProperty("role").GetString() == "Organiser"
                    ? RedirectToAction("Dashboard", "Organiser")
                    : RedirectToAction("Index", "Participant");
            }
            ViewBag.Error = "Invalid email or password.";
            return View(model);
        }

        [HttpGet] public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var response = await _api.PostAsync("/api/auth/register", model);
            if (response.IsSuccessStatusCode) return RedirectToAction("Login");
            ViewBag.Error = "Registration failed. Email might exist.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
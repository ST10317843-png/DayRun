using DayRun.Web.Filters;
using DayRun.Web.Services;
using DayRun.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DayRun.Web.Controllers
{
    [RoleAuthorize("Organiser")]
    public class OrganiserController : Controller
    {
        private readonly ApiService _api;
        public OrganiserController(ApiService api) { _api = api; }

        public async Task<IActionResult> Dashboard()
        {
            var events = await _api.GetAsync<List<dynamic>>("/api/events");
            return View(events);
        }

        [HttpGet] public IActionResult CreateEvent() => View();

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel model, IFormFile Banner)
        {
            var response = await _api.PostAsync("/api/events", model);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var eventId = System.Text.Json.JsonDocument.Parse(content).RootElement.GetProperty("eventId").GetInt32();

                if (Banner != null)
                {
                    await _api.PostFileAsync($"/api/events/{eventId}/banner", Banner);
                }
                return RedirectToAction("Dashboard");
            }
            return View(model);
        }
    }
}
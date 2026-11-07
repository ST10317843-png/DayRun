using DayRun.Web.Filters;
using DayRun.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DayRun.Web.Controllers
{
    [RoleAuthorize("Participant")]
    public class ParticipantController : Controller
    {
        private readonly ApiService _api;
        public ParticipantController(ApiService api) { _api = api; }

        public async Task<IActionResult> Index()
        {
            var events = await _api.GetAsync<List<dynamic>>("/api/events");
            return View(events);
        }

        public async Task<IActionResult> MyEvents()
        {
            var enrolments = await _api.GetAsync<List<dynamic>>("/api/enrolments/mine");
            return View(enrolments);
        }

        public async Task<IActionResult> MyResults()
        {
            var results = await _api.GetAsync<List<dynamic>>("/api/results/mine");
            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> EnterEvent(int categoryId)
        {
            var data = new { categoryId = categoryId };
            var response = await _api.PostAsync("/api/enrolments", data);
            return RedirectToAction("MyEvents");
        }
    }
}
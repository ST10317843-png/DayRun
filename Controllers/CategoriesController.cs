using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DayRun.API.Data;
using DayRun.API.Models;
using System.Security.Claims;

namespace DayRun.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context) { _context = context; }

        [HttpGet("event/{eventId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetForEvent(int eventId)
        {
            return await _context.Categories.Where(c => c.EventId == eventId).ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ev = await _context.Events.FindAsync(category.EventId);
            if (ev == null) return NotFound("Event not found.");
            if (ev.OrganiserId.ToString() != userId) return Forbid();

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForEvent), new { eventId = category.EventId }, category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existing = await _context.Categories.Include(c => c.Event)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (existing == null) return NotFound();
            if (existing.Event!.OrganiserId.ToString() != userId) return Forbid();

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.EntryFee = category.EntryFee;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _context.Categories.Include(c => c.Event)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();
            if (category.Event!.OrganiserId.ToString() != userId) return Forbid();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
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
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EventsController(ApplicationDbContext context) { _context = context; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events
                .Include(e => e.Categories)
                .Include(e => e.Venue)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Categories)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.EventId == id);
            if (ev == null) return NotFound();
            return ev;
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Event>> CreateEvent(Event ev)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ev.OrganiserId = int.Parse(userId!);
            ev.CreatedAt = DateTime.UtcNow;
            ev.UpdatedAt = DateTime.UtcNow;
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = ev.EventId }, ev);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEvent(int id, Event ev)
        {
            if (id != ev.EventId) return BadRequest();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existing = await _context.Events.FindAsync(id);
            if (existing == null) return NotFound();
            if (existing.OrganiserId.ToString() != userId) return Forbid();

            existing.Name = ev.Name;
            existing.Description = ev.Description;
            existing.Date = ev.Date;
            existing.Location = ev.Location;
            existing.Distance = ev.Distance;
            existing.EventType = ev.EventType;
            existing.MaxParticipants = ev.MaxParticipants;
            existing.Status = ev.Status;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();
            if (ev.OrganiserId.ToString() != userId) return Forbid();
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
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
    public class EnrolmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EnrolmentsController(ApplicationDbContext context) { _context = context; }

        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<Enrolment>> CreateEnrolment(Enrolment enrolment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            enrolment.ParticipantId = int.Parse(userId!);
            enrolment.EnrolmentDate = DateTime.UtcNow;
            enrolment.Status = "Confirmed";

            var existing = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.ParticipantId == enrolment.ParticipantId
                                       && e.CategoryId == enrolment.CategoryId);
            if (existing != null) return Conflict("Already enrolled in this category.");

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMyEnrolments), new { id = enrolment.EnrolmentId }, enrolment);
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetMyEnrolments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Enrolments
                .Where(e => e.ParticipantId.ToString() == userId)
                .Include(e => e.Category).ThenInclude(c => c.Event)
                .ToListAsync();
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolmentsForEvent(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound("Event not found.");
            if (ev.OrganiserId.ToString() != userId) return Forbid();

            return await _context.Enrolments
                .Where(e => e.Category!.EventId == eventId)
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .ToListAsync();
        }
    }
}
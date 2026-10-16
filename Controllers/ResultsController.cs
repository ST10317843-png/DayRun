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
    public class ResultsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ResultsController(ApplicationDbContext context) { _context = context; }

        [HttpPost("enrolment/{enrolmentId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Result>> CaptureResult(int enrolmentId, Result result)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrolment = await _context.Enrolments
                .Include(e => e.Category).ThenInclude(c => c.Event)
                .FirstOrDefaultAsync(e => e.EnrolmentId == enrolmentId);
            if (enrolment == null) return NotFound("Enrolment not found.");
            if (enrolment.Category!.Event!.OrganiserId.ToString() != userId) return Forbid();

            var existing = await _context.Results.FirstOrDefaultAsync(r => r.EnrolmentId == enrolmentId);
            if (existing != null)
            {
                existing.FinishTime = result.FinishTime;
                existing.Position = result.Position;
                existing.Status = result.Status;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok(existing);
            }

            result.EnrolmentId = enrolmentId;
            result.CreatedAt = DateTime.UtcNow;
            result.UpdatedAt = DateTime.UtcNow;
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            return Ok(result);
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<IEnumerable<Result>>> GetMyResults()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Results
                .Where(r => r.Enrolment!.ParticipantId.ToString() == userId)
                .Include(r => r.Enrolment).ThenInclude(e => e.Category).ThenInclude(c => c.Event)
                .ToListAsync();
        }

        [HttpGet("event/{eventId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Result>>> GetResultsForEvent(int eventId)
        {
            return await _context.Results
                .Where(r => r.Enrolment!.Category!.EventId == eventId)
                .Include(r => r.Enrolment).ThenInclude(e => e.Participant)
                .OrderBy(r => r.Position)
                .ToListAsync();
        }
    }
}
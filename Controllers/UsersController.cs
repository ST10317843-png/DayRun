using DayRun.API.Data;
using DayRun.API.Models;
using DayRun.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DayRun.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context) { _context = context; }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(int.Parse(userId!));
            if (user == null) return NotFound();
            return Ok(new { user.UserId, user.Email, user.FirstName, user.LastName, user.Role });
        }
        [HttpPost("me/profile-picture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file, [FromServices] BlobService blobService)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(int.Parse(userId!));
            if (user == null) return NotFound();

            var url = await blobService.UploadFileAsync(file);
            user.ProfilePictureUrl = url;
            await _context.SaveChangesAsync();
            return Ok(new { url });
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(UserProfileUpdateDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(int.Parse(userId!));
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
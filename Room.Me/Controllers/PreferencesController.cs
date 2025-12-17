using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using Room.Me.Dtos;
using System.Threading.Tasks;

namespace Room.Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly RoomMeDbContext _context;

        public PreferencesController(RoomMeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPreferences()
        {
            var preferences = await _context.Preferences.ToListAsync();
            var groupedPreferences = preferences
                .GroupBy(p => p.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => new PreferenceItemDto
                    {
                        Id = p.Id,
                        Label = p.Label,
                        Value = p.Value
                    }).ToList()
                );

            return Ok(groupedPreferences);
        }

        // Endpoint -> Post: api/preferences/user

        [HttpPost("user")]
        public async Task<IActionResult> SaveUserPreferences([FromBody] UserPreferencesUpdateDto dto)
        {

            var user = await _context.Users
                .Include(u => u.UserPreferences)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null) return NotFound("Usuario no encontrado");

            var existingPreferenceIds = await _context.Preferences
                .Where(p => dto.PreferenceIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            if (user.UserPreferences.Any())
            {
                _context.UserPreferences.RemoveRange(user.UserPreferences);
            }

            var newPreferences = existingPreferenceIds.Select(prefId => new UserPreference
            {
                UserId = dto.UserId,
                PreferenceId = prefId
            });

            await _context.UserPreferences.AddRangeAsync(newPreferences);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Preferencias actualizadas correctamente" });
        }

        // Endpoint -> Get: api/preferences
        // Endpoint para sacar de un usuario en específico -> Get: api/preferences/user/id del usuario
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPreferences(int userId)
        {
            var userPreferences = await _context.UserPreferences
                .Where(up => up.UserId == userId)
                .Include(up => up.Preference)
                .Select(up => new PreferenceItemDto
                {
                    Id = up.Preference.Id,
                    Label = up.Preference.Label,
                    Value = up.Preference.Value
                })
                .ToListAsync();

            return Ok(userPreferences);
        }
    }
}
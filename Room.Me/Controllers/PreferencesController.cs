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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var prefs = await _context.Preferences
                    .Select(p => new PreferenceDto
                    {
                        Id = p.Id,
                        PetFriendly = p.PetFriendly,
                        AllowSmoking = p.AllowSmoking,
                        AllowGuests = p.AllowGuests,
                        AllowParties = p.AllowParties,
                        LikesMusic = p.LikesMusic,
                        IsOrganized = p.IsOrganized,
                        WakesUpEarly = p.WakesUpEarly,
                        IsQuiet = p.IsQuiet
                    })
                    .ToListAsync();

                return Ok(new
                {
                    message = "Preferencias obtenidas",
                    preferences = prefs
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "No se pudo obtener las preferencias."
                });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreatePreferenceDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var pref = new Preferences
                {
                    PetFriendly = dto.PetFriendly,
                    AllowSmoking = dto.AllowSmoking,
                    AllowGuests = dto.AllowGuests,
                    AllowParties = dto.AllowParties,
                    LikesMusic = dto.LikesMusic,
                    IsOrganized = dto.IsOrganized,
                    WakesUpEarly = dto.WakesUpEarly,
                    IsQuiet = dto.IsQuiet
                };

                _context.Preferences.Add(pref);
                await _context.SaveChangesAsync();

                return Ok(new   
                {
                    message = "Preferencia creada correctamente",
                    preference = new
                    {
                        pref.Id,
                        pref.PetFriendly,
                        pref.AllowSmoking,
                        pref.AllowGuests,
                        pref.AllowParties,
                        pref.LikesMusic,
                        pref.IsOrganized,
                        pref.WakesUpEarly,
                        pref.IsQuiet
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al crear la preferencia."
                });
            }

        }
    }
}

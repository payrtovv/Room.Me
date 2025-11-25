using Microsoft.AspNetCore.Mvc;
using Room.Me.Data;

namespace Room.Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly RoomMeDbContext _context;

        public AccessController(RoomMeDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Usuario registrado correctamente",
                user
            });
        }
    }
}

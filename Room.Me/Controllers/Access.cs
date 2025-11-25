using Microsoft.AspNetCore.Mvc;

using Room.Me.Data;
using System;

namespace Room.Me.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class Access : ControllerBase
    {
        private readonly RoomMeDbContext _context;

        public Access(RoomMeDbContext context)
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

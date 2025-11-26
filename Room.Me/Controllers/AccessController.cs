using Microsoft.AspNetCore.Mvc;
using Room.Me.Data;
using Room.Me.Models;
using Microsoft.AspNetCore.Identity;


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

        [HttpGet("Login")]
        public IActionResult Login([FromBody] LoginDto login) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var User = _context.Users.FirstOrDefault(u => u.Email == login.Email);


            if (User == null)
            {
                return NotFound(new
                {
                    message = "Usuario no encontrado"
                });
            }
            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(User, User.Password, login.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Contraseña incorrecta" });
            }

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                User = new
                {
                    User.Id,
                    User.Email
                }
            });



        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Email = dto.Email,
                Name = dto.Name
                , Surname = dto.Surname
            };

            user.Password = hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Usuario registrado correctamente",
                user = new
                {
                    user.Id,
                    user.Email,
                    user.Name,
                    user.Surname
                }
            });
        }
    }
}

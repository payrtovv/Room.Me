using Microsoft.AspNetCore.Mvc;
using Room.Me.Data;
using Room.Me.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace Room.Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        //base de datos
        private readonly RoomMeDbContext _context;
        //servicio de verificacion de email
        private readonly SendgidEmailServices _emailService;

        //accesos a la base de datos y al servicio de email
        public AccessController(RoomMeDbContext context, SendgidEmailServices emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        //metodo para iniciar sesion
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto login) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Buscar usarario por email
            var User = _context.Users.FirstOrDefault(u => u.Email == login.Email);

            //Si no existe el usuario
            if (User == null)
            {
                return NotFound(new
                {
                    message = "Usuario no encontrado"
                });
            }
            //hash de la contraseña
            var hasher = new PasswordHasher<User>();

            //Comparar contraseñas
            var result = hasher.VerifyHashedPassword(User, User.Password, login.Password);

            //si la contraseña es incorrecta
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


        //metodo para enviar codigo de verificacion
        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody]Emaildto dto)
        {
            //Validar correo

            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
            {
                return BadRequest(new { message = "Correo inválido" });
            }

            try
            {
                // Crear código aleatorio de 4 dígitos
                string code = new Random().Next(1000, 9999).ToString();

                // Enviar correo
                await _emailService.SendEmailCode(dto.Email, code);

                return Ok(new
                {
                    message = "Código enviado correctamente"
                });
            }
            catch (Exception ex)
            {
                // No se pudo enviar el correo

                return BadRequest(new
                {
                    message = "No se pudo enviar el correo",
                    error = ex.Message
                });
            }
        }



        //metodo para registrar usuario
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            //validar modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //buscar usuario por email
            var User = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            //si el usuario ya existe
            if (User != null)
            {
                return Conflict(new
                {
                    message = "Esta email ya esta registrado"
                });
            }

            //hash de la contraseña
            var hasher = new PasswordHasher<User>();

            //crear usuario
            var user = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname
            };

            //guardar usuario con contraseña hasheada

            user.Password = hasher.HashPassword(user, dto.Password);

            //Subir a la base de datos
            _context.Users.Add(user);
            _context.SaveChanges();

            //retornar mensaje 
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

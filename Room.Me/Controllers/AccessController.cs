using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using Room.Me.Dtos;
using Room.Me.Services;
using System.Security.Claims;



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
        //Servucio de JWT
        private readonly JwtService _config;
        //Servicio de imagnes
        private readonly Room.Me.Services.ImageService _imageService;



        //accesos a la base de datos y al servicio de email
        public AccessController(RoomMeDbContext context, SendgidEmailServices emailService, JwtService config, Room.Me.Services.ImageService imageService)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
            _imageService = imageService;
        }


        //metodo para iniciar sesion
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login) {
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

            //Generar token JTW
            var Token = _config.GenerateToken(User.Id, User.Email);

            //Comparar contraseñas
            var result = hasher.VerifyHashedPassword(User, User.Password, login.Password);

            //si la contraseña es incorrecta
            if (result == PasswordVerificationResult.Failed)
            {
                    return Unauthorized();
            }

            if (User.IsVerified == false)
            {
                return Forbid();
            }

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                token = Token,
                User = new
                {
                    User.Id,
                    User.Email
                }
            });
        }


        //metodo para enviar codigo de verificacion
        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody]String email)
        {
            //Validar correo

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                return BadRequest(new { message = "Correo inválido" });
            }

            try
            {
                //Crear numero aleatorio de 4 digitos

                string code = new Random().Next(1000, 9999).ToString();

                //Verificar si el usuario existe
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                //si el usuario existe, guardar el codigo y la expiracion
                if (user != null)
                {
                    user.VerificationCode = code;
                    user.CodeExpiration = DateTime.UtcNow.AddMinutes(10); // expira en 10 min
                    await _context.SaveChangesAsync();
                }


                // Enviar correo con servicio de email
                await _emailService.SendEmailCode(email, code);

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

        [HttpPost("VerificateCode")]
        public async Task<IActionResult> VerificateCode([FromBody] VerifyCodeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //Buscar usuario por email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

                //si el usuario no existe
                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "Usuario no encontrado"
                    });
                }

                //si el codigo no es igual o ya expiro

                if (user.VerificationCode != dto.VerificationCode || user.CodeExpiration < DateTime.UtcNow)
                {
                    return BadRequest(new
                    {
                        message = "Código inválido o expirado"
                    });
                }else
                {
                    //marcar usuario como verificado
                    user.IsVerified = true;
                    user.VerificationCode = null; // Borrar el código después de la verificación
                    user.CodeExpiration = null; // Borrar la expiración después de la verificación
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        message = "Código verificado correctamente"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Error al verificar el código",
                    error = ex.Message
                });
            }
        }



        //metodo para registrar usuario
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
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
                    Surname = dto.Surname,
                    Gender = dto.Gender,
                    Age = dto.Age,
                    IsVerified = false
                };

                //guardar usuario con contraseña hasheada

                user.Password = hasher.HashPassword(user, dto.Password);

                // se guarda al usuario para obtener su id
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // crear las preferencias del usuario
                if (dto.PreferenceIds != null && dto.PreferenceIds.Any())
                {
                    var userPreferences = dto.PreferenceIds.Select(prefId => new UserPreference
                    {
                        UserId = user.Id, 
                        PreferenceId = prefId
                    }).ToList();

                    await _context.UserPreferences.AddRangeAsync(userPreferences);
                    await _context.SaveChangesAsync();
                }

            
                await SendCode(dto.Email);

                //retornar mensaje 
                return Ok(new
                {
                    message = "Usuario registrado correctamente",
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.Name,
                        user.Surname,
                        user.Gender,
                        user.Age,
                    }
                });
                               
            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error interno. Inténtalo más tarde."
                });

            }   


        }

        // metodo para agregar el icon

        [HttpPost("upload-photo/{userId}")]
        public async Task<IActionResult> UploadProfilePhoto(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha enviado ninguna imagen.");

            var imageUrl = await _imageService.UploadImageAsync(file);

            // actualiza la base de datos 
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("Usuario no encontrado.");

            user.ProfilePictureUrl = imageUrl;
            await _context.SaveChangesAsync();

            return Ok(new { url = imageUrl });
        }

        [Authorize]
        [HttpPost("EditUser")]
        public async Task<ActionResult> EditUser([FromBody] EditUserDto Dto)
        {
            try
            {
                //Obtener UserID del token
                var userIdString = User.FindFirst("id")?.Value;

                //Ver si el token es valido
                if (userIdString == null)
                    return Unauthorized(new { message = "Token inválido" });

                //Convertir a int
                int userId = int.Parse(userIdString);


                //Buscar User por id

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "Usuario no encontrado"
                    });
                }
                else
                {
                    //Editar datos del usuario
                    user.Name = Dto.Name;
                    user.Surname = Dto.Surname;
                    user.Gender = Dto.Gender;
                    user.Age = Dto.Age;
                    if (Dto.Photo != null && Dto.Photo.Length > 0)
                    {
                        string newPhotoUrl = await _imageService.UploadImageAsync(Dto.Photo);
                        user.ProfilePictureUrl = newPhotoUrl;
                    }

                    //Subir cambios a la base de datos
                    await _context.SaveChangesAsync();  

                    return Ok(new
                    {
                        message = "Usuario editado correctamente",
                        user = new
                        {
                            user.Name,
                            user.Surname,
                            user.Gender,
                            user.Age,
                            imageUrl = user.ProfilePictureUrl
                        }
                    });
                }
            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error interno. Inténtalo más tarde."
                });

            }
        }

        //obeter informacion de un usuario por Id
        [Authorize]
        [HttpGet("GetInfoUser/{Id}")]
        public async Task<IActionResult> GetInfoUser(int Id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserPreferences)
                        .ThenInclude(up => up.Preference)
                    .FirstOrDefaultAsync(u => u.Id == Id);

                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(new
                {
                    user = new
                    {
                        user.Id,
                        Preferences = user.UserPreferences.Select(up => new
                        {
                            up.Preference.Id,
                            up.Preference.Label
                        }).ToList()
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ocurrió un error interno." });
            }
        }



        [Authorize]
        [HttpGet("GetLocalUser")]
        public async Task<IActionResult> GetLocalUser()
        {
            try
            {
                var userid = GetUserId();

                //Buscamos usuario por Id
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid);

                //Si no se encuentra
                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "Usuario no encontrado"
                    });
                }
                else
                {
                    //Si se encuentra 
                    return Ok(new
                    {
                        user = new
                        {
                            user.Id,
                            user.Email,
                            user.Name,
                            user.Surname,
                            user.Age,
                            user.Gender,
                            imageUrl = user.ProfilePictureUrl,
                            Preferences = _context.UserPreferences
                                .Where(up => up.UserId == user.Id)
                                .Select(up => new
                                {
                                    up.Preference.Id,
                                    up.Preference.Category,
                                    up.Preference.Value,
                                    up.Preference.Label
                                }).ToList()
                        }
                    });
                }

            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error interno. Inténtalo más tarde."
                });
            }

        }


        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);

            return Ok(exists);
        }
        [Authorize]
        [HttpDelete("DeleteAccount")]
        public async Task<ActionResult> DeleteAccount()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            //Borrar rooms del usuario
            var rooms = await _context.Rooms
                .Where(r => r.IdUserHost == userId)
                .ToListAsync();

            _context.Rooms.RemoveRange(rooms);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok("Cuenta eliminada correctamente");
        }


        private int? GetUserId()
        {
            var userId = User.FindFirstValue("id");
            if (int.TryParse(userId, out int id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }
    }
}

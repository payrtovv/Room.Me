using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Room.Me.Data;
using Room.Me.Dtos;
using Room.Me.Models;
using System.Security.Claims;


namespace Room.Me.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomMeDbContext _Context;


        public RoomsController(RoomMeDbContext context)
        {
            _Context = context;
        }

        //Crear habitacion
        [HttpPost("CreateRoom")]
        public async Task<ActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Obtiene el ID del usuario autenticado desde el JWT (claim "id")
                var userId = User.FindFirst("id")?.Value;

                //Mira si el user id es null
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                //Mira si puede cambiar a int y lo guarda en la variable id
                if (!int.TryParse(userId, out int id))
                {
                    return Unauthorized(new { message = "ID de usuario inválido" });
                }

                //crea una variable room
                var room = new Rooms
                {
                    IdUserHost = id,
                    Description = dto.Description,
                    M2Space = dto.M2Space,
                    Price = dto.Price,
                    Direccion = dto.Direccion,
                    City = dto.City,
                    NearTransport = dto.NearTransport,
                    NearCollege = dto.NearCollege
                };

                foreach (var featureid in dto.FeatureIds)
                {
                    room.RoomFeatures.Add(new RoomFeature{
                        FeatureId = featureid
                    });
                }

                //aniadimos la Room
                _Context.Rooms.Add(room);
                await _Context.SaveChangesAsync();

                //Retornamos mensaje de exito
                return Ok(new
                {
                    message = "Habitacion registrada exitosamente",
                    room = new
                    {
                        IdUserHost = id,
                        room.Description,
                        room.M2Space,
                        room.Price,
                        room.Direccion,
                        room.City,
                        room.NearTransport,
                        room.NearCollege,
                        room.RoomFeatures
                    }
                });

            }
            catch (Exception Ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error interno. Inténtalo más tarde.",
                    error = "Error interno del servidor"
                });
            }
        }

        //Para habitaciones propias
        [Authorize]
        [HttpGet("Getlocal/{idRoom}")]
        public async Task<ActionResult> GetLocalRoom(int idroom)
        {
            var Userid = GetUserId();
            if (Userid == null)
                return Unauthorized();


            var room = await _Context.Rooms
                .Where(r => r.IdRoom == idroom && r.IdUserHost == Userid)
                .Select(r => new
                {
                    r.Description,
                    r.M2Space,
                    r.Price,
                    r.Direccion,
                    r.City,
                    r.NearTransport,
                    r.NumOfBathrooms,
                    r.NearCollege,
                    r.RoomFeatures
                })
                .FirstOrDefaultAsync();

            if (room == null)
                return NotFound();


            return Ok(room);
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
         

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
                    NearCollege = dto.NearCollege,
                    IncludesElectricity = dto.IncludesElectricity,
                    IncludesWater = dto.IncludesWater,
                    IncludesInternet = dto.IncludesInternet,
                    IncludesGas = dto.IncludesGas,
                    IncludesCleaning = dto.IncludesCleaning
                };

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
                        room.IncludesElectricity,
                        room.IncludesWater,
                        room.IncludesInternet,
                        room.IncludesGas,
                        room.IncludesCleaning
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
    }
}
         

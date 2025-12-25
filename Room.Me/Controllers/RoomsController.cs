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
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomMeDbContext _Context;


        public RoomsController(RoomMeDbContext context)
        {
            _Context = context;
        }

        [Authorize]
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

        [Authorize]
        [HttpPost("CreateRule")]
        public async Task<ActionResult> CreateRule([FromBody] CreateRuleDto RuleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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

                //Buscar la habitacion y ver si pertenece al usuario 
                var room = await _Context.Rooms.FirstOrDefaultAsync(r => r.IdRoom == RuleDto.RoomId && r.IdUserHost == id);

                if (room == null)
                {
                    return NotFound(new 
                    {
                        message = "Habitación no encontrada o no pertenece al usuario",
                        roomIdRecibido = RuleDto.RoomId
                    });
                }


                //Crear la rule
                var rule = new Rule
                {
                    Name = RuleDto.RuleName,
                    IsMandatory = false,
                    CreatedByUserId = id
                };

                //Subir la RUle

                _Context.Rules.Add(rule);
                await _Context.SaveChangesAsync();


                //Relacionar la regla con la room
                var roomRule = new RoomRule
                {
                    RoomId=RuleDto.RoomId,
                    RuleId = rule.Id,
                    Value = true
                };

                _Context.RoomRules.Add(roomRule);
                await _Context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Regla creada con exito",
                    nombre = RuleDto.RuleName,
                });

            }catch(Exception Ex)
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
         

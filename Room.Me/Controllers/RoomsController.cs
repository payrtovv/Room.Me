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
                var userId = GetUserId();

                //Mira si el user id es null
                if (userId == null)
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                int id = userId.Value;

                //crea una variable room
                var room = new Rooms
                {
                    IdUserHost = id,
                    Description = dto.Description,
                    Type = dto.Type,
                    Street = dto.Street,
                    Direccion = dto.Direccion,
                    City = dto.City,
                    Latitud = dto.Latitud,
                    Longitud =dto.Longitud,
                    NumOfBathrooms = dto.NumOfBathrooms,
                    NumOfRooms = dto.NumOfRooms,
                    NumOfParkingSpaces = dto.NumOfParkingSlots,
                    M2Space = dto.M2Space,
                    Price = dto.Price,
                    NearTransport = dto.NearTransport,
                    NearCollege = dto.NearCollege
                };

                foreach (var featureid in dto.FeatureIds)
                {
                    room.RoomFeatures.Add(new RoomFeature{
                        FeatureId = featureid
                    });
                }


                foreach (var ruleDto in dto.Rules)
                {
                    var rule = new Rule
                    {
                        Name = ruleDto.RuleName,
                        CreatedByUserId = userId,
                        Room = room
                    };

                    _Context.Rules.Add(rule);
                }


                //aniadimos la Room
                _Context.Rooms.Add(room);
                await _Context.SaveChangesAsync();

                //Retornamos mensaje de exito
                return Ok(new
                {
                    message = "Habitacion registrada exitosamente"
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
                    r.Type,
                    r.Street,
                    r.Direccion,
                    r.City,
                    r.Latitud,
                    r.Longitud,
                    r.NumOfBathrooms,
                    r.NumOfRooms,
                    r.NumOfParkingSpaces,
                    r.M2Space,
                    r.Price,
                    r.NearTransport,
                    r.NearCollege,
                    Rules = r.Rules.Select(rr => new
                    {
                        rr.Name
                    }).ToList(),

                    Feature = r.RoomFeatures.Select(rf => new
                    {
                        rf.Feature.Id,
                        rf.Feature.Name,
                        rf.Feature.Category,
                        rf.Feature.Key
                    }).ToList()
                    
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
         

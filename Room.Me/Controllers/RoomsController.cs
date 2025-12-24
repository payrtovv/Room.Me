using Microsoft.AspNetCore.Mvc;
using Room.Me.Data;
using Room.Me.Dtos;
using Room.Me.Models;

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

        [HttpPost("CreateRoom")]
        public async Task<ActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var room = new Rooms
                {
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

                _Context.Rooms.Add(room);
                await _Context.SaveChangesAsync();


                return Ok(new
                {
                    message = "Habitacion registrada exitosamente",
                    room = new
                    {
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
                    error = Ex.Message
                });
            }
        }
    }
}
         

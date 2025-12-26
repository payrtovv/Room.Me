using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using Room.Me.Dtos;
using Room.Me.Models;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace Room.Me.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RuleController : Controller
    {

        private readonly RoomMeDbContext _Context;


        public RuleController(RoomMeDbContext context)
        {
            _Context = context;
        }

        //Metodo para el nombre y el value de una regla en especifico 
        [HttpPost("UpdateValueRule")]
        public async Task<ActionResult> UpdateRule([FromBody] UpdateRuleDto dto)
        {
            try
            {
                //sacamos el id del JWT
                var id = GetUserId();
                if (id == null)
                    return Unauthorized();


                //Sacamos de la tabla rules la rule que tenga el nombre dado
                var rule = await _Context.Rules.FirstOrDefaultAsync(r => r.Name == dto.RuleName);
                if (rule == null)
                    return NotFound(new { message = "Regla no encontrada" });
                //Verificamos que el usuario tenga la room comparando las ids 
                var room = await _Context.Rooms.FirstOrDefaultAsync(r => r.IdRoom == dto.Roomid && r.IdUserHost == id);
                if (room == null)
                    return NotFound(new { message = "Habitación no encontrada o no pertenece al usuario" });
                //Con las anteriores variables sacamos el roomRules 
                var roomRule = await _Context.RoomRules.FirstOrDefaultAsync(r => r.RoomId == room.IdRoom && r.RuleId == rule.Id);
                if (roomRule == null)
                    return NotFound(new { message = "La regla no está asociada a la habitación" });

                //Cambiamos al valor 
                roomRule.Value = dto.Value;

                rule.Name = dto.NewRuleName;


                //Guardamos cambios
                await _Context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Value de la regla cambiado",
                    Nombre = dto.RuleName,
                    NuevoNombre = rule.Name,
                    Habitacion = room.IdRoom
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error interno. Inténtalo más tarde.",
                    error = "Error interno del servidor"
                });
            }
        }

        //Metodo para crear regla y asignarla a la habitacion
        [HttpPost("CreateRule")]
        public async Task<ActionResult> CreateRule([FromBody] CreateRuleDto RuleDto)
        {
            try
            {
                //Usamos la funcion creada para obtener el id del susario por el JWT
                var id = GetUserId();
                if (id == null)
                    return Unauthorized();
            

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
                    RoomId = RuleDto.RoomId,
                    RuleId = rule.Id,
                    Value = true
                };

                //Lo subimos

                _Context.RoomRules.Add(roomRule);
                await _Context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Regla creada con exito",
                    nombre = RuleDto.RuleName,
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

        //Para obtener el listado de las reglas de una habitacion
        [HttpGet("getrules/{roomId}")]
        public async Task<ActionResult> ListRules(int roomid) 
        {
            //Usamos la funcion creada para obtener el id del susario por el 
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            //Buscamos la habitacino
            var room = await _Context.Rooms.FirstOrDefaultAsync(r => r.IdRoom == roomid && r.IdUserHost == userId);

            if (room == null)
            {
                return NotFound(new
                {
                    message = "Habitación no encontrada o no pertenece al usuario",
                    roomIdRecibido = roomid
                });
            }
            //Hacemos una lista de las reglas de la habitacion
            var roomRules = await _Context.RoomRules
                //Concidan las id de la room en RoomRules
                .Where(rr => rr.RoomId == room.IdRoom)
                //Incluir las reglas
                .Include(rr => rr.Rule)
                .Select(rr => new
                {
                    rr.Rule.Id,
                    rr.Rule.Name,
                    rr.Value,
                })
                .ToListAsync();

            return Ok(roomRules);
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

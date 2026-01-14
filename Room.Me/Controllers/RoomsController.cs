using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Room.Me.Data;
using Room.Me.Dtos;
using Room.Me.Models;
using Room.Me.Services;
using SendGrid.Helpers.Mail;
using System.Security.Claims;


namespace Room.Me.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomMeDbContext _Context;
        private readonly ImageService _imageService;

        public RoomsController(RoomMeDbContext context, ImageService imageService)
        {
            _Context = context;
            _imageService = imageService;
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
                    Title = dto.Title,
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
                    r.Title,
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
                        rr.Name,
                        rr.Id
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

        [HttpPost("updateRoom")]
        public async Task<ActionResult> updateRoom(UpdateRoomDto dto)
        {
            var userid = GetUserId();

            var room = await _Context.Rooms
                .Include(r => r.RoomFeatures)
                .Include(r => r.Rules)
                .FirstOrDefaultAsync(r => r.IdRoom == dto.Id && r.IdUserHost == userid);


            if (room == null)
                return NotFound(new { message = "Habitación no encontrada o no autorizada." });

            // Actualizar los campos de la habitación
            room.Title = dto.Title;
            room.Description = dto.Description;
            room.Type = dto.Type;
            room.Street = dto.Street;
            room.Direccion = dto.Direccion;
            room.City = dto.City;
            room.Latitud = dto.Latitud;
            room.Longitud = dto.Longitud;
            room.NumOfBathrooms = dto.NumOfBathrooms;
            room.NumOfRooms = dto.NumOfRooms;
            room.NumOfParkingSpaces = dto.NumOfParkingSlots;
            room.M2Space = dto.M2Space;
            room.Price = dto.Price;
            room.NearTransport = dto.NearTransport;
            room.NearCollege = dto.NearCollege;

            // Eliminamos características que ya no están en la lista
            var featuresToRemove = room.RoomFeatures
                .Where(rf => !dto.FeatureIds.Contains(rf.FeatureId))
                .ToList();

            _Context.RoomFeatures.RemoveRange(featuresToRemove);

            // Agregar nuevas características
            foreach (var featureid in dto.FeatureIds)
            {
                //si no existe la caracteristica, la agregamos
                if (!room.RoomFeatures.Any(rf => rf.FeatureId == featureid))
                {
                    room.RoomFeatures.Add(new RoomFeature
                    {
                        FeatureId = featureid
                    });
                }
            }

            //For each para revisar las reglas que manda el fron
            foreach (var Rule in dto.Rules)
            {
                //buscamos si la regla ya existe en la base de datos
                var existingRule = room.Rules.FirstOrDefault(r => r.Id == Rule.RuleId);
                //si existe, actualizamos el nombre
                if (existingRule != null)
                {
                    existingRule.Name = Rule.RuleName;
                }
                else

                //si no existe, la agregamios
                room.Rules.Add(new Rule
                {
                    Name = Rule.RuleName,
                    CreatedByUserId = userid,
                });
            }

            //buscamos las reglas que ya no estan en el dto para eliminarlas
            var ruleIdsFromDto = dto.Rules
                //Como puede venir una regla nueva sin id, filtramos los nulos
                .Where(r => r.RuleId != null)
                .Select(r => r.RuleId)
                .ToList();
            //lista de reglas a eliminar
            var rulesToDelete = room.Rules
                //Si la regla en la base de datos no esta en la lista del dto, la eliminamos
                //mayor a 0 por que las que se agregan apenas estan con id 0 temporalmente
                .Where(r => r.Id > 0 && !ruleIdsFromDto.Contains(r.Id))
                .ToList();

            _Context.Rules.RemoveRange(rulesToDelete);

            await _Context.SaveChangesAsync();

            return Ok(new { message = "Habitación actualizada exitosamente." });

        }

        [HttpDelete("deleteRoom/{RoomId}")]
        public async Task<ActionResult> DeleteRoom(int RoomId)
        {
            //obtenemos el id del usuario
            var userid = GetUserId();
            
            var room = await _Context.Rooms
                .FirstOrDefaultAsync(r => r.IdRoom == RoomId && r.IdUserHost == userid);
            
            if (room == null)
                return NotFound(new { message = "Habitación no encontrada o no autorizada." });
            
            _Context.Rooms.Remove(room);
            
            await _Context.SaveChangesAsync();
            
            return Ok(new { message = "Habitación eliminada exitosamente." });
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
        // POST: api/rooms/IdUSuario/media
        [HttpPost("{idRoom}/media")]
        [Authorize] 
        public async Task<IActionResult> UploadRoomMedia(int idRoom, List<IFormFile> files)
        {
            // validar
            var room = await _Context.Rooms.FindAsync(idRoom);
            if (room == null) return NotFound("La habitación no existe.");

            if (files == null || files.Count == 0)
                return BadRequest("No se enviaron archivos.");

            var uploadedMedia = new List<RoomMedia>();

            foreach (var file in files)
            {
                 var url = await _imageService.UploadImageAsync(file);

                if (!string.IsNullOrEmpty(url))
                {
                    uploadedMedia.Add(new RoomMedia
                    {
                        RoomId = idRoom,
                        Url = url,
                        ContentType = file.ContentType 
                    });
                }
            }

            if (uploadedMedia.Count > 0)
            {
                await _Context.RoomMedia.AddRangeAsync(uploadedMedia);
                await _Context.SaveChangesAsync();
            }

            return Ok(new { message = "Archivos subidos", data = uploadedMedia });
        }
    }
}
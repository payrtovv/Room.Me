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
        public async Task<ActionResult> CreateRoom([FromForm] CreateRoomDto dto)
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
                    address = dto.address,
                    Lat = dto.Lat,
                    Lng =dto.Lng,
                    Bathrooms = dto.Bathrooms,
                    Bedrooms = dto.Bedrooms,
                    ParkingSpaces = dto.ParkingSpaces,
                    Surface = dto.Surface,
                    Price = dto.Price,
                };

                //Hacemos un foreach para recorrer la lista de Ids para subirlas
                //Nunca va a ser null por que en el dto esta inicializada
                foreach (var featureid in dto.FeatureIds)
                {
                    room.RoomFeatures.Add(new RoomFeature{
                        FeatureId = featureid
                    });
                }

                //Hacemos un foreach para recorrer la lista de Rules para subirlas
                //Nunca va a ser null por que en el dto esta inicializada
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
                //Subimos a la base de datos
                _Context.Rooms.Add(room);
                await _Context.SaveChangesAsync();

                //Creamos una lista para mandar las fotos 
                var uploadedMedia = new List<RoomMedia>();

                //Si existen
                if (dto.Files != null && dto.Files.Count > 0)
                {
                    //Recorremos con for each
                    foreach (var file in dto.Files)
                    {
                        //Usamos el servicio de image service para subir 
                        var url = await _imageService.UploadImageAsync(file);
                        if (!string.IsNullOrEmpty(url))
                        {
                            uploadedMedia.Add(new RoomMedia
                            {
                                RoomId = room.IdRoom,
                                Url = url,
                                ContentType = file.ContentType
                            });
                        }
                    }
                }              
                //Subimos las fotos

                await _Context.RoomMedia.AddRangeAsync(uploadedMedia);
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
                    message = Ex.Message,
                    stack = Ex.InnerException?.Message

                });
            }
        }

        [HttpPost("updateRoom")]
        public async Task<ActionResult> updateRoom(UpdateRoomDto dto)
        {
            //Sacamos el id del user
            var userid = GetUserId();

            //buscamos la habitacion del dto y vemos si le pertenece
            var room = await _Context.Rooms
                .Include(r => r.RoomFeatures)
                .Include(r => r.Rules)
                .FirstOrDefaultAsync(r => r.IdRoom == dto.Id && r.IdUserHost == userid);

            //No se encontro 
            if (room == null)
                return NotFound(new { message = "Habitación no encontrada o no autorizada." });

            // Actualizar los campos de la habitación
            room.Title = dto.Title;
            room.Description = dto.Description;
            room.Type = dto.Type;
            room.address = dto.address;
            room.Lng = dto.Lng;
            room.Lat = dto.Lat;
            room.Bathrooms = dto.Bathrooms;
            room.Bedrooms = dto.Bedrooms;
            room.ParkingSpaces = dto.ParkingSpaces;
            room.Surface = dto.Surface;
            room.Price = dto.Price;


            // Eliminamos características que ya no están en la lista
            var featuresToRemove = room.RoomFeatures
                .Where(rf => !dto.FeatureIds.Contains(rf.FeatureId)) //Las que no tengan el id del dto
                .ToList();

            _Context.RoomFeatures.RemoveRange(featuresToRemove); //las quitamos de la base de datos

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

            //Hacemos una lista que tenga la media que se va a eliminar
            var MediaToremove = room.Media.Where(mr => !dto.Files.Contains(mr.Id)).ToList(); //devolvemos las que no estan en el dto

            //Las eliminamos
            _Context.RoomMedia.RemoveRange(MediaToremove);

            //Creamos una lista para las nuevas imagenes
            var uploadedMedia = new List<RoomMedia>();

            //recorremos las nuevas imagenes
            foreach (var file in dto.NewFiles)
            {
                //Las subimos con el servicio 
                var url = await _imageService.UploadImageAsync(file);

                if (!string.IsNullOrEmpty(url))
                {
                    uploadedMedia.Add(new RoomMedia
                    {
                        RoomId = dto.Id,
                        Url = url,
                        ContentType = file.ContentType
                    });
                }
            }
            //Subimos las nuevas imagenes
            await _Context.RoomMedia.AddRangeAsync(uploadedMedia);
            await _Context.SaveChangesAsync();

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

        //Para habitaciones propias
        [Authorize]
        [HttpGet("Getlocal/{idRoom}")]
        public async Task<ActionResult> GetLocalRoom(int idroom)
        {
            //Validamos el user
            var Userid = GetUserId();
            if (Userid == null)
                return Unauthorized();

            //Bucacamos la habitacion y selecionamos lo que queremos debolver
            var room = await _Context.Rooms
                .Where(r => r.IdRoom == idroom && r.IdUserHost == Userid)
                .Select(r => new
                {
                    r.Title,
                    r.Description,
                    r.Type,
                    r.address,
                    r.Lng,
                    r.Lat,
                    r.Bathrooms,
                    r.Bedrooms,
                    r.ParkingSpaces,
                    r.Surface,
                    r.Price,

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
                    }).ToList(),

                    Media = r.Media.Select(m => new
                    {
                        m.Id,
                        m.Url,
                        m.ContentType
                    }).ToList()


                })
                .FirstOrDefaultAsync();

            if (room == null)
                return NotFound();


            return Ok(room);
        }
        
        [HttpGet("GetAllRooms")]
        public async Task<ActionResult> GetAllRooms()
        {
            //Obtenemos el id del user
            var IdUser = GetUserId();

            if (IdUser == null)
                return Unauthorized();


            //Buscamos las habitaciones que no tengan el id del user 
            var rooms = await _Context.Rooms.
            Where(r=> r.IdUserHost != IdUser).
            Select(r => new
            {
                Host = new
                {
                    r.IdUserHost,
                    r.user.Name,
                    r.user.ProfilePictureUrl
                },
                 r.IdRoom,
                 r.Title,
                 r.Description,
                 r.Price,
                 r.Type,
                 r.address,
                 r.Bathrooms,
                 r.Bedrooms,
                 r.ParkingSpaces,
                 r.Surface,
                 Media = r.Media.Select(m => new
                 {
                     m.Id,
                     m.Url,
                     m.ContentType
                 }).FirstOrDefault() //Para que solo nos de la primera
             }).ToListAsync();

            return Ok(rooms);
        }

        [AllowAnonymous]
        [HttpGet("GetAllRoomsAnonymous")]
        public async Task<ActionResult> GetAllRoomsAnonymous()
        {
            var rooms = await _Context.Rooms.
            Select(r => new
            {
                Host = new
                {
                    r.IdUserHost,
                    r.user.Name,
                    r.user.ProfilePictureUrl
                },
                r.IdRoom,
                r.Title,
                r.Description,
                r.Price,
                r.Type,
                r.address,
                r.Bathrooms,
                r.Bedrooms,
                r.ParkingSpaces,
                r.Surface,
                Media = r.Media.Select(m => new
                {
                    m.Id,
                    m.Url,
                    m.ContentType
                }).FirstOrDefault() //Para que solo nos de la primera
            }).ToListAsync();

            return Ok(rooms);
        }


        

        [HttpDelete("deleteRoom/{RoomId}")]
        public async Task<ActionResult> DeleteRoom(int RoomId)
        {
            //obtenemos el id del usuario
            var userid = GetUserId();
            //Bucamos la habitacion
            var room = await _Context.Rooms
                .FirstOrDefaultAsync(r => r.IdRoom == RoomId && r.IdUserHost == userid);
            
            if (room == null)
                return NotFound(new { message = "Habitación no encontrada o no autorizada." });
            
            //Quitamos y como esta en cascade se borra lo relacionado a la room
            //Osea las rules y las features
            _Context.Rooms.Remove(room);
            
            await _Context.SaveChangesAsync();
            
            return Ok(new { message = "Habitación eliminada exitosamente." });
        }


        [HttpGet("GetMyRooms")]
        public async Task<ActionResult> GetMyRooms()
        {
            var userid = GetUserId();

            if (userid == null)
            {
                return Unauthorized(new
                { message = "Token inválido" });
            }

            var rooms = await _Context.Rooms.Where(r => r.IdUserHost == userid).
                Select(r => new
                {
                    r.IdRoom,
                    r.Title,
                    r.Description,
                    r.Price,
                    r.Type,
                    r.address,
                    r.Bathrooms,
                    r.Bedrooms,
                    r.ParkingSpaces,
                    r.Surface,
                    Media = r.Media.Select(m => new
                    {
                        m.Id,
                        m.Url,
                        m.ContentType
                    }).FirstOrDefault() //Para que solo nos de la primera
                }).ToListAsync();

            if (!rooms.Any())
                return (NoContent());

            return Ok(rooms);
        }
    
        [HttpGet("GetRoom/{IdRoom}")]
        public async Task<ActionResult> GetRoom(int IdRoom)
        {
            var room = await _Context.Rooms
                .Where(r => r.IdRoom == IdRoom)
                .Select(r => new
                {
                    Host = new
                    {
                        r.IdUserHost,
                        r.user.Name,
                        r.user.ProfilePictureUrl
                    },
                    r.IdUserHost,
                    r.Title,
                    r.Description,
                    r.Type,
                    r.address,
                    r.Lng,
                    r.Lat,
                    r.Bathrooms,
                    r.Bedrooms,
                    r.ParkingSpaces,
                    r.Surface,
                    r.Price,

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
                    }).ToList(),

                    Media = r.Media.Select(m => new
                    {
                        m.Id,
                        m.Url,
                        m.ContentType
                    }).ToList()


                })
                .FirstOrDefaultAsync();

            if (room == null)
                return NotFound();

            return Ok(room);
        }


        [AllowAnonymous]
        [HttpGet("GetRooms")]
        public async Task<ActionResult> GetRooms(
            //Esto va a salir en el endpoint para pedir
            string? category,
            int? bedrooms,
            int? priceRangelow,
            int? priceRageHigh,
            int? parkingSpaces,
            int? bathrooms,
            float? surface)
        {
            //El query para el filtro
            var query = _Context.Rooms.AsQueryable();

            //si esta null
            if(category != null)
            {
                //que conincida
                query = query.Where(r => r.Type == category);
            }
            //asi con todos los demas

            if (bedrooms != null)
            {
                query = query.Where(r => r.Bedrooms == bedrooms);
            }


            if (priceRageHigh != null && priceRangelow != null)
            {
                query = query.Where(r => r.Price < priceRageHigh && r.Price > priceRangelow);
            }


            if (parkingSpaces != null)
            {
                query = query.Where(r => r.ParkingSpaces == parkingSpaces);
            }


            if (bathrooms != null)
            {
                query = query.Where(r => r.Bathrooms == bathrooms);
            }

            if (surface != null)
            {
                query = query.Where(r => r.Surface == surface);
            }

            //Con el query buscamos las habitaciones 
            var result = await query.Select(r => new
            {
                //Lo que va a regresar
                Host = new
                {
                    r.IdUserHost,
                    r.user.Name,
                    r.user.ProfilePictureUrl
                },
                r.IdRoom,
                r.Title,
                r.Description,
                r.Price,
                Media = r.Media.Select(m => new
                {
                    m.Id,
                    m.Url,
                    m.ContentType
                }).FirstOrDefault() //Para que solo nos de la primera
            }
            //Para que nos de una lista
            ).ToListAsync();

            //Si no se encontro
            if (!result.Any())
                return NoContent();
            //Debolvemos la lista
            return Ok(result);
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using System.Security.Claims;

namespace Room.Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class MessagesController : ControllerBase
    {
        private readonly RoomMeDbContext _context;

        public MessagesController(RoomMeDbContext context)
        {
            _context = context;
        }

        //Endpoint
        // GET: api/messages/Id del User
        [HttpGet("{otherUserId}")]
        public async Task<IActionResult> GetConversation(int otherUserId)
        {
            try
            {
                // obtener el ID del usuario 
                var myIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (myIdClaim == null)
                    return Unauthorized("Token inválido.");

                var myId = int.Parse(myIdClaim.Value);

                //buscar los mensajes entre los dos usuarios
                var messages = await _context.Messages
                    .Where(m => (m.SenderId == myId && m.ReceiverId == otherUserId) ||
                                (m.SenderId == otherUserId && m.ReceiverId == myId))
                    .OrderBy(m => m.SentAt) 
                    .Select(m => new
                    {
                        m.Id,
                        m.Content,
                        m.SentAt,
                        m.SenderId,
                        m.ReceiverId,
                        IsMine = m.SenderId == myId
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar mensajes: " + ex.Message);
            }
        }
    }
}
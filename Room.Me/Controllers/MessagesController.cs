using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using Room.Me.Dtos;
using Room.Me.Hubs;
using Room.Me.Models;
using Room.Me.Services;
using System.Security.Claims;

namespace Room.Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly RoomMeDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ImageService _imageService;

        public MessagesController(
            RoomMeDbContext context,
            IHubContext<ChatHub> hubContext,
            ImageService imageService)
        {
            _context = context;
            _hubContext = hubContext;
            _imageService = imageService;
        }

        [HttpGet("{otherUserId}")]
        public async Task<IActionResult> GetConversation(int otherUserId)
        {
            try
            {
                // obtener el ID del usuario 
                var myIdClaim = User.FindFirst("id");
                if (myIdClaim == null) return Unauthorized("Token inválido.");

                var myId = int.Parse(myIdClaim.Value);

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
                        IsMine = m.SenderId == myId,
                        m.ImageUrl
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar mensajes: " + ex.Message);
            }
        }

        // POST: api/messages
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageDto dto, IFormFile? file)
        {
            try
            {
                var myIdClaim = User.FindFirst("id");
                if (myIdClaim == null) return Unauthorized();
                var senderId = int.Parse(myIdClaim.Value);
                               
                string imageUrl = null;
                if (file != null && file.Length > 0)
                {
                    imageUrl = await _imageService.UploadImageAsync(file);
                }

                //buscar los mensajes entre los dos usuarios
                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = dto.ReceiverId,
                    Content = dto.Content,
                    SentAt = DateTime.UtcNow,
                    ImageUrl = imageUrl 
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // se notifica al receptor
                await _hubContext.Clients.User(dto.ReceiverId.ToString())
                    .SendAsync("ReceiveMessage", senderId, message.Content, imageUrl);

                return Ok(new { Message = "Enviado", Data = message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar mensajes: " + ex.Message);
            }
        }

        // GET: api/messages/conversation/Id del usuario
        [HttpGet("conversation/{otherUserId:int}")]
        public async Task<IActionResult> GetConversationHistory(int otherUserId)
        {
            try
            {
                var myIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
                if (myIdClaim == null) return Unauthorized("Token inválido.");
                var myId = int.Parse(myIdClaim.Value);

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
                        IsMine = m.SenderId == myId,
                        m.ImageUrl 
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar historial: " + ex.Message);
            }
        }

        // GET: api/messages/inbox

        [HttpGet("inbox")]
        public async Task<IActionResult> GetInbox()
        {
            try
            {
                var myIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
                if (myIdClaim == null) return Unauthorized("Token inválido.");
                var myId = int.Parse(myIdClaim.Value);

                // se mete a  sender y receiver para sacar los nombres 
                var allMyMessages = await _context.Messages
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Where(m => m.SenderId == myId || m.ReceiverId == myId)
                    .OrderByDescending(m => m.SentAt) 
                    .ToListAsync();

                var inboxList = allMyMessages
                    .GroupBy(m => m.SenderId == myId ? m.ReceiverId : m.SenderId)
                    .Select(g =>
                    {
                        var lastMsg = g.First();

                        var otherUser = (lastMsg.SenderId == myId) ? lastMsg.Receiver : lastMsg.Sender;

                        return new
                        {
                            otherUserId = otherUser.Id,
                            otherUserName = otherUser.Name, 
                            otherUserProfilePic = otherUser.ProfilePictureUrl,
                            lastMessage = (lastMsg.SenderId == myId ? "Tú: " : "") + lastMsg.Content, 
                            lastMessageTime = lastMsg.SentAt,
                            isMine = lastMsg.SenderId == myId
                        };
                    })
                    .ToList();

                return Ok(inboxList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar bandeja de entrada: " + ex.Message);
            }
        }

    } 
}
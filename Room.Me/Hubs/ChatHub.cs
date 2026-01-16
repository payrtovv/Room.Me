using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using Room.Me.Models;
using System.Security.Claims;

namespace Room.Me.Hubs
{
    [Authorize] //para que solo los user logeados se contecten
    public class ChatHub : Hub
    {
        private readonly RoomMeDbContext _context;

        public ChatHub(RoomMeDbContext context)
        {
            _context = context;
        }
        public async Task SendMessage(int receiverId, string messageContent)
        {
            // identificar al emisor
            var senderIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          
            if (string.IsNullOrEmpty(senderIdStr)) return;
            var senderId = int.Parse(senderIdStr);

            // se guarda el mensaje en la base de datos
            var newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = messageContent,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Envio del mensaje 
            // enviar al receptor
            await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", new
            {
                senderId = senderId,
                message = messageContent,
                sentAt = newMessage.SentAt
            });

            // notificar al emisor que el mensaje se envio
            await Clients.Caller.SendAsync("MessageSent", new
            {
                receiverId = receiverId,
                message = messageContent,
                sentAt = newMessage.SentAt
            });
        }

        // se une al usuario a un grupo con su Id para enviar mensajes directos
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }
    }
}
namespace Room.Me.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public string? ImageUrl { get; set; }
        public DateTime SentAt { get; set; }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool IsMine { get; set; }
    }

    public class SendMessageDto
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
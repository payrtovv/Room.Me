using Room.Me.Data;
using System.ComponentModel.DataAnnotations;

namespace Room.Me.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        [Required]
        public string Content { get; set; }

        //public string ImageUrl { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;


    }
}
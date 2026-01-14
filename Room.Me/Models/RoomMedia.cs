using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.Me.Models
{
    public class RoomMedia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } 

        public string ContentType { get; set; } 

        // relacion con Rooms
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Rooms Room { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Room.Me.Models
{
    public class RoomRule
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Value { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public Rooms Room { get; set; } = null!;
    }
}

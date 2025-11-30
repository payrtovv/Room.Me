using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Room.Me.Models
{
    public class RoomRule
    {
        [Key]
        public int Id { get; set; }

        // FK hacia Room
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Rooms Room { get; set; } = null!;

        // FK hacia Rule
        [ForeignKey("Rule")]
        public int RuleId { get; set; }
        public Rule Rule { get; set; } = null!;

        public bool Value { get; set; }
    }
}

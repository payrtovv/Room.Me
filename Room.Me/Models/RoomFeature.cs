using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.Me.Models
{
    public class RoomFeature
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Rooms Room { get; set; }

        [ForeignKey("Feature")]
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }

    }
}

using Microsoft.AspNetCore.Http.Features;

namespace Room.Me.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RoomFeature> RoomFeatures { get; set; }

    }
}

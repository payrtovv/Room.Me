using Microsoft.AspNetCore.Http.Features;

namespace Room.Me.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public String Category { get; set; }

        public String Key { get; set; }

        public ICollection<RoomFeature> RoomFeatures { get; set; }

    }
}

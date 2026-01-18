namespace Room.Me.Dtos
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public String Type { get; set; }

        public String address { get; set; }

        public float Lat { get; set; }

        public float Lng { get; set; }

        public int Bathrooms { get; set; }

        public int Bedrooms { get; set; }

        public int ParkingSpaces { get; set; }

        public String Description { get; set; }

        //Tamanio en M2 dela habitacion
        public float Surface { get; set; }

        public float Price { get; set; }

        public List<int> FeatureIds { get; set; } = new();

        public List<CreateRoomRuleDto> Rules { get; set; } = new();

        public List<int> Files { get; set; } = new();

        public List<IFormFile> NewFiles { get; set; } = new();


    }
}

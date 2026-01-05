namespace Room.Me.Dtos
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }
        
        public String Title { get; set; }
        public String Description { get; set; }

        public String Type { get; set; }

        public String Street { get; set; }

        public String Direccion { get; set; }

        public String City { get; set; }

        public float Latitud { get; set; }

        public float Longitud { get; set; }

        public int NumOfBathrooms { get; set; }

        public int NumOfRooms { get; set; }

        public int NumOfParkingSlots { get; set; }

        //Tamanio en M2 dela habitacion
        public float M2Space { get; set; }

        public float Price { get; set; }

        //Si esta cerca de transporte
        public bool NearTransport { get; set; }

        public bool NearCollege { get; set; }

        public List<int> FeatureIds { get; set; } = new();

        public List<CreateRoomRuleDto> Rules { get; set; } = new();
    }
}

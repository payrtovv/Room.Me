namespace Room.Me.Dtos
{
    public class CreateRoomDto
    {

        public String Description { get; set; }

        //Tamanio en M2 dela habitacion
        public float M2Space { get; set; }

        public float Price { get; set; }

        //Calles
        public String Direccion { get; set; }

        public String City { get; set; }

        //Si esta cerca de transporte
        public bool NearTransport { get; set; }

        public bool NearCollege { get; set; }

        //Servicios

        public bool IncludesElectricity { get; set; }
        public bool IncludesWater { get; set; }
        public bool IncludesInternet { get; set; }
        public bool IncludesGas { get; set; }
        public bool IncludesCleaning { get; set; }
    }
}

using Room.Me.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.Me.Models
{
    //Rooms y no rooms por que hay problemas por el nombre Room.Me
    public class Rooms
    {
        [Key]
        public int IdRoom { get; set; }

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

        //Si esta ocupada
        public bool State { get; set; }
        //El usuario dueno de la habitacion
        public int IdUserHost { get; set; }

        //Esto se refiere al dueno de la habitacion
        [ForeignKey("IdUserHost")]
        public User user { get; set; }

        public List<RoomRule> RoomRule { get; set; } = new();

    }
}

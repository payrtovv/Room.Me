using Room.Me.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.Me.Models
{
    //Rooms y no room por que hay problemas por el nombre Room.Me
    public class Rooms
    {
        [Key]
        public int IdRoom { get; set; }

        public String Title { get; set; }

        public String Type { get; set;}

        public String Street { get; set; }

        public String Direccion { get; set; }

        public String City { get; set; }

        public float Latitud { get; set; }

        public float Longitud { get; set; }

        public int NumOfBathrooms { get; set; }

        public int NumOfRooms { get; set; }

        public int NumOfParkingSpaces { get; set; }

        public String Description { get; set; }

        //Tamanio en M2 dela habitacion
        public float M2Space { get; set; }

        public float Price { get; set; }

        //Si esta cerca de transporte
        public bool NearTransport { get; set; }

        public bool NearCollege { get; set; }

        //Si esta visible
        public bool State { get; set; }
        //El usuario dueno de la habitacion
        public int IdUserHost { get; set; }

        //Esto se refiere al dueno de la habitacion
        [ForeignKey("IdUserHost")]
        public User user { get; set; }

        public List<Rule> Rules{ get; set; } = new();

        public List<RoomFeature> RoomFeatures { get; set; } = new List<RoomFeature>();


    }
}

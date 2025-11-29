using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.Me.Models
{
    public class Room
    {
        [Key]
        public int IdRoom { get; set; }

        public String Description { get; set; }

        //Tamanio en M2
        public float M2Space { get; set; }

        public bool PrivateBathroom { get; set; }

        public String Direccion { get; set; }

        public String City { get; set; }

        //Si esta ocupada
        public bool State { get; set; }

        public int IdUserHost { get; set; }

        //Esto se refiere al dueno de la habitacion
        [ForeignKey("IdUserHost")]
        public User user { get; set; }
    }
}

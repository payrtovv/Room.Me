using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Room.Me.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }

    }
}

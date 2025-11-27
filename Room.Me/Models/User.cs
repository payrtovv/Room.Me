using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Room.Me.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Gender { get; set; }
        public int Age { get; set; }
        //nullable 
        public string? VerificationCode { get; set; }
        //nullable 
        public DateTime? CodeExpiration { get; set; }
        public bool IsVerified { get; set; }

    }
}

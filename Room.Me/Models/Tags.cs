using System.ComponentModel.DataAnnotations;
namespace Room.Me.Data;
    public class Tags
    {
        public int Id { get; set; }

        [Required]
        public string Personality { get; set; }

        [Required]
        public string Routine { get; set; }

        [Required]
        public string Cleanliness { get; set; }

        [Required]
        public string Pets { get; set; }

        [Required]
        public string Visits { get; set; }

        [Required]
        public string Smoking { get; set; }
        public int UserId { get; set; }
    }



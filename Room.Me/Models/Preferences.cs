using Room.Me.Models;
using System.ComponentModel.DataAnnotations;

namespace Room.Me.Data
{
    public class Preference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Category { get; set; } 

        [Required]
        public string Label { get; set; }    

        [Required]
        public string Value { get; set; }    

        public ICollection<UserPreference> UserPreferences { get; set; }
    }
}

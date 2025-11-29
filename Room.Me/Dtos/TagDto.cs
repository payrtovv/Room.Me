using Microsoft.AspNetCore.Mvc;

namespace Room.Me.Dtos
{
    public class TagDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Personality { get; set; }
        public string Routine { get; set; }
        public string Cleanliness { get; set; }
        public string Pets { get; set; }
        public string Visits { get; set; }
        public string Smoking { get; set; }
    }
}

namespace Room.Me.Dtos
{
    public class RegisterDto
    {
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }

        public String Gender { get; set; }
        public int Age { get; set; }

        //para los ids de las preferencias
        public List<int> PreferenceIds { get; set; }
    }
}

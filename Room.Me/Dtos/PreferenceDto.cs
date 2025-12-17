namespace Room.Me.Dtos
{

    public class PreferenceItemDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class UserPreferencesUpdateDto
    {
        public int UserId { get; set; }
        public List<int> PreferenceIds { get; set; }
    }
}
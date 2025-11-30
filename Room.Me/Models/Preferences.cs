using Room.Me.Models;
using System.ComponentModel.DataAnnotations;
namespace Room.Me.Data;
public class Preferences
{
    public int Id { get; set; }
    public bool PetFriendly { get; set; }
    public bool AllowSmoking { get; set; }
    public bool AllowGuests { get; set; }
    public bool AllowParties { get; set; }
    public bool? LikesMusic { get; set; }
    public bool? IsOrganized { get; set; }
    public bool? WakesUpEarly { get; set; }
    public bool? IsQuiet { get; set; }
}

using Room.Me.Models;
using System.ComponentModel.DataAnnotations;

public class Rule
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public int? CreatedByUserId { get; set; }

    public int RoomId { get; set; }
    public Rooms Room { get; set; } = null!;


}

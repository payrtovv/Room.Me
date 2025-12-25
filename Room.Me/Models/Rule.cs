using Room.Me.Models;
using System.ComponentModel.DataAnnotations;

public class Rule
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public bool IsMandatory { get; set; }

    public int? CreatedByUserId { get; set; }

    public List<RoomRule> RoomRules { get; set; } = new();
}

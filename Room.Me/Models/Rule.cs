using System.ComponentModel.DataAnnotations;

public class Rule
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }            
}

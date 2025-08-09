using System.ComponentModel.DataAnnotations;

namespace AspNetFinalProject.Entities;

public class Tag
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = null!;

    [MaxLength(20)]
    public string? Color { get; set; }

    public ICollection<TagCard> TagCards { get; set; } = new List<TagCard>();
}
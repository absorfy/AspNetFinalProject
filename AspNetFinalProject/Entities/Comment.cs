using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public Card Card { get; set; } = null!;
    
    [Required]
    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;
    
    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public string? DeletedByUserId { get; set; }
    [ForeignKey(nameof(DeletedByUserId))]
    public UserProfile? DeletedByUser { get; set; }
}
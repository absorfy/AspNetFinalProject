using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class BoardList
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public Board Board { get; set; } = null!;

    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = null!;
    public DateTime CreatingTimestamp { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public string? DeletedByUserId { get; set; }
    [ForeignKey(nameof(DeletedByUserId))]
    public UserProfile? DeletedByUser { get; set; }
    
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
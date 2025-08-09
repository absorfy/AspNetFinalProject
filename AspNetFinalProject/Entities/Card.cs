using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class Card
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid BoardListId { get; set; }
    [ForeignKey(nameof(BoardListId))]
    public BoardList BoardList { get; set; } = null!;

    [Required]
    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;

    public DateTime CreatingTimestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }

    public DateTime? Deadline { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedByUserId { get; set; }

    [ForeignKey(nameof(DeletedByUserId))]
    public UserProfile? DeletedByUser { get; set; }
    
    public ICollection<CardParticipant> Participants { get; set; } = new List<CardParticipant>();
    
    public ICollection<TagCard> TagCards { get; set; } = new List<TagCard>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public ICollection<CardAttachment> Attachments { get; set; } = new List<CardAttachment>();
}
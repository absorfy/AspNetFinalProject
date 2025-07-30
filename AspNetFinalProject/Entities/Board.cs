using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class Board
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int WorkSpaceId { get; set; }
    [ForeignKey(nameof(WorkSpaceId))]
    public WorkSpace WorkSpace { get; set; } = null!;
    
    [Required]
    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;
    
    public DateTime CreatingTimestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public BoardVisibility Visibility { get; set; } = BoardVisibility.Private;

    public DateTime? DeletedAt { get; set; }

    public string? DeletedByUserId { get; set; }
    [ForeignKey(nameof(DeletedByUserId))]
    public UserProfile? DeletedByUser { get; set; }
    
    public ICollection<BoardParticipant> Participants { get; set; } = new List<BoardParticipant>();
    
    public ICollection<BoardList> Lists { get; set; } = new List<BoardList>();
}
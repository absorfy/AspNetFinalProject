using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class WorkSpace
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;
    
    public DateTime CreatingTimestamp { get; set; } = DateTime.UtcNow;
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    public WorkSpaceVisibility Visibility { get; set; } = WorkSpaceVisibility.Private;
    
    public DateTime? DeletedAt { get; set; }
    public string? DeletedByUserId { get; set; }

    [ForeignKey(nameof(DeletedByUserId))]
    public UserProfile? DeletedByUser { get; set; }
    
    public ICollection<WorkSpaceParticipant> Participants { get; set; } = new List<WorkSpaceParticipant>();
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}
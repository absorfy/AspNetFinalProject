using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class BoardParticipant
{
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public Board Board { get; set; } = null!;
    public string UserProfileId { get; set; } = null!;
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;
    
    public BoardRole Role { get; set; } = BoardRole.Viewer;
    
    public DateTime JoiningTimestamp { get; set; } = DateTime.UtcNow;
}
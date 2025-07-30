using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class WorkSpaceParticipant
{
    public int WorkSpaceId { get; set; }
    [ForeignKey(nameof(WorkSpaceId))]
    public WorkSpace WorkSpace { get; set; } = null!;
    
    public string UserProfileId { get; set; } = null!;
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;

    public WorkSpaceRole Role { get; set; } = WorkSpaceRole.Member;

    public DateTime JoiningTimestamp { get; set; } = DateTime.UtcNow;
}
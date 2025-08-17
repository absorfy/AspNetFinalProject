using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class WorkSpaceParticipant
{
    public Guid WorkSpaceId { get; set; }
    [ForeignKey(nameof(WorkSpaceId))]
    public WorkSpace WorkSpace { get; set; } = null!;
    
    public string UserProfileId { get; set; } = null!;
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;

    public ParticipantRole Role { get; set; } = ParticipantRole.Member;

    public DateTime JoiningTimestamp { get; set; } = DateTime.UtcNow;
}
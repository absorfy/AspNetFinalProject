
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class WorkSpaceParticipantDto
{
    public string UserProfileId { get; set; }
    public string WorkSpaceId { get; set; }
    public string Username { get; set; }
    public WorkSpaceRole Role { get; set; }
    public bool IsChanging { get; set; }
    public DateTime JoiningTimestamp { get; set; }
}
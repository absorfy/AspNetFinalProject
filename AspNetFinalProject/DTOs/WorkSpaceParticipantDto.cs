
namespace AspNetFinalProject.DTOs;

public class WorkSpaceParticipantDto
{
    public string UserProfileId { get; set; }
    public string WorkSpaceId { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTime JoiningTimestamp { get; set; }
}
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class WorkSpaceParticipantDto
{
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTime JoiningTimestamp { get; set; }
}
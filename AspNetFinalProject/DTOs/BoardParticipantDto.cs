using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class BoardParticipantDto
{
    public string UserProfileId { get; set; }
    public string BoardId { get; set; }
    public string Username { get; set; }
    public ParticipantRole Role { get; set; }
    public bool IsChanging { get; set; }
    public DateTime JoiningTimestamp { get; set; }
}
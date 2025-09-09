using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class ParticipantRoleDto
{
    public ParticipantRole Value { get; set; }
    public string Text { get; set; }
    public bool Hidden { get; set; }
}
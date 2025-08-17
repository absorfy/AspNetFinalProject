using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Mappers;

public static class ParticipantRoleMapper
{
    public static ParticipantRoleDto CreateDto(ParticipantRole role)
    {
        return new ParticipantRoleDto
        {
            Value = role,
            Text = role switch
            {
                ParticipantRole.Admin => "Адмін",
                ParticipantRole.Member => "Учасник",
                ParticipantRole.Owner => "Власник",
                ParticipantRole.Viewer => "Спостерігач",
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            },
            Hidden = role switch
            {
                ParticipantRole.Owner => true,
                _ => false
            }
        };
    }
}
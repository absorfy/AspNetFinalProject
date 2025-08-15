using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Mappers;

public static class WorkSpaceRoleMapper
{
    public static WorkSpaceRoleDto CreateDto(WorkSpaceRole role)
    {
        return new WorkSpaceRoleDto
        {
            Value = (int)role,
            Text = role switch
            {
                WorkSpaceRole.Admin => "Адмін",
                WorkSpaceRole.Member => "Учасник",
                WorkSpaceRole.Owner => "Власник",
                WorkSpaceRole.Viewer => "Спостерігач",
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            },
            Hidden = role switch
            {
                WorkSpaceRole.Owner => true,
                _ => false
            }
        };
    }
}
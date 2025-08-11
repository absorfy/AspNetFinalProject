using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class WorkSpaceParticipantMapper
{
    public static WorkSpaceParticipantDto CreateDto(WorkSpaceParticipant workSpaceParticipant)
    {
        return new WorkSpaceParticipantDto
        {
            UserProfileId = workSpaceParticipant.UserProfileId,
            WorkSpaceId = workSpaceParticipant.WorkSpaceId.ToString(),
            JoiningTimestamp = workSpaceParticipant.JoiningTimestamp,
            Username = workSpaceParticipant.UserProfile?.Username ??
                   workSpaceParticipant.UserProfile?.IdentityUser.UserName ?? "Unknown",
            Role = workSpaceParticipant.Role.ToString()
        };
    }
}
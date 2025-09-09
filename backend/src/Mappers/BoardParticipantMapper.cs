using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public class BoardParticipantMapper
{
    public static BoardParticipantDto CreateDto(BoardParticipant boardParticipant, bool isChanging)
    {
        return new BoardParticipantDto
        {
            UserProfileId = boardParticipant.UserProfileId,
            BoardId = boardParticipant.BoardId.ToString(),
            JoiningTimestamp = boardParticipant.JoiningTimestamp,
            Username = boardParticipant.UserProfile?.Username ??
                       boardParticipant.UserProfile?.IdentityUser.UserName ?? "Unknown",
            Role = boardParticipant.Role,
            IsChanging = isChanging,
        };
    }
}
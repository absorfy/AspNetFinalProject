using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class UserProfileMapper
{

    public static UserProfileDto CreateDto(UserProfile userProfile)
    {
        return new UserProfileDto
        {
            IdentityId = userProfile.IdentityId,
            Username = userProfile.Username,
            Email = userProfile.IdentityUser.Email,
            PersonalInfo = PersonalInfoMapper.CreateDto(userProfile.PersonalInfo),
        };
    }
    
    public static void UpdateEntity(UserProfile userProfile, UpdateUserProfileDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(userProfile);
        ArgumentNullException.ThrowIfNull(updateDto);
        
        userProfile.Username = updateDto.Username;
        //userProfile.AvatarUrl = updateDto.AvatarUrl;
        PersonalInfoMapper.UpdateEntity(userProfile.PersonalInfo, updateDto.PersonalInfo);
    }
}
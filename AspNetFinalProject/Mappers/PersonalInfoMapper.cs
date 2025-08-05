using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class PersonalInfoMapper
{
    public static void UpdateEntity(PersonalInfo personalInfo, UpdatePersonalInfoDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(personalInfo);
        ArgumentNullException.ThrowIfNull(updateDto);

        personalInfo.Name = updateDto.Name;
        personalInfo.Surname = updateDto.Surname;
        personalInfo.BirthDate = updateDto.BirthDate;
        personalInfo.Gender = updateDto.Gender;
        personalInfo.PhoneNumber = updateDto.PhoneNumber;
        personalInfo.About = updateDto.About;
    }
}
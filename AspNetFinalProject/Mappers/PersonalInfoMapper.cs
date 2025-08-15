using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Mappers;

public static class PersonalInfoMapper
{
    public static PersonalInfoDto CreateDto(PersonalInfo personalInfo)
    {
        return new PersonalInfoDto
        {
            Name = personalInfo.Name,
            Surname = personalInfo.Surname,
            BirthDate = personalInfo.BirthDate,
            Gender = (int)personalInfo.Gender,
            PhoneNumber = personalInfo.PhoneNumber,
            About = personalInfo.About,
        };
    }
    
    public static void UpdateEntity(PersonalInfo personalInfo, UpdatePersonalInfoDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(personalInfo);
        ArgumentNullException.ThrowIfNull(updateDto);

        personalInfo.Name = updateDto.Name;
        personalInfo.Surname = updateDto.Surname;
        personalInfo.BirthDate = updateDto.BirthDate;
        if (Enum.TryParse(updateDto.Gender.ToString(), out GenderType gender))
        {
            personalInfo.Gender = gender;
        }
        else throw new Exception("Invalid gender");
        personalInfo.PhoneNumber = updateDto.PhoneNumber;
        personalInfo.About = updateDto.About;
    }
}
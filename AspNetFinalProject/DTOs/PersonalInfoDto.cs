using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class PersonalInfoDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime? BirthDate { get; set; }
    public GenderType Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? About { get; set; }
}

public class UpdatePersonalInfoDto
{
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Surname { get; set; }
    public DateTime? BirthDate { get; set; }
    public GenderType Gender { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [MaxLength(500)]
    public string? About { get; set; }
}
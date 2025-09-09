using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.DTOs;

public class UserProfileDto
{
    public string IdentityId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    //public string? AvatarUrl { get; set; }
    public PersonalInfoDto PersonalInfo { get; set; }
}

public class UpdateUserProfileDto
{
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    public string? Username { get; set; }
    //public string? AvatarUrl { get; set; }
    [Required]
    public UpdatePersonalInfoDto PersonalInfo { get; set; }
}
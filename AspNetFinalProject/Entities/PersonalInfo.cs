using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class PersonalInfo
{
    [Key, ForeignKey(nameof(UserProfile))]
    public string UserProfileId { get; set; } = null!;

    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Surname { get; set; }
    public DateTime? BirthDate { get; set; }
    public GenderType Gender { get; set; } = GenderType.None;
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [MaxLength(500)]
    public string? About { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
    
}
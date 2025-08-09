using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class Subscription
{
    [Required]
    public string UserProfileId { get; set; } = null!;
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;

    [Required]
    public EntityTargetType EntityType { get; set; }
    [Required]
    [MaxLength(36)]
    public string EntityId { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
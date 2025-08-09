using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class UserActionLog
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string UserProfileId { get; set; } = null!;
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;
    
    [Required]
    public EntityTargetType EntityType { get; set; }
    
    [Required]
    [MaxLength(36)]
    public string EntityId { get; set; } = null!;
    [Required]
    public UserActionType ActionType { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
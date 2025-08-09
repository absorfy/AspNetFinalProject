using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class Notification
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string UserProfileId { get; set; }
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;

    [Required]
    public Guid UserActionLogId { get; set; }
    
    [ForeignKey(nameof(UserActionLogId))]
    public UserActionLog UserActionLog { get; set; } = null!;

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
}
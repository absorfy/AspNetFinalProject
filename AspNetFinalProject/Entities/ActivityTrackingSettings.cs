using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class ActivityTrackingSettings
{
    [Key, ForeignKey(nameof(UserProfile))]
    public string UserProfileId { get; set; } = null!;
    
    public bool TrackCards { get; set; } = true;
    public bool TrackBoards { get; set; } = true;
    public bool TrackLists { get; set; } = true;

    public UserProfile UserProfile { get; set; } = null!;
}
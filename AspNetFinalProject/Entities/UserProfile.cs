using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Enums;
using Microsoft.AspNetCore.Identity;

namespace AspNetFinalProject.Entities;

public class UserProfile
{
    [Key, ForeignKey(nameof(IdentityUser))]
    public string IdentityId { get; set; } = null!;
    
    [MaxLength(10)]
    public string? Language { get; set; }
    public ThemeType Theme { get; set; } = ThemeType.Light;
    [MaxLength(50)]
    public string? Username { get; set; }
    [MaxLength(2048)]
    public string? AvatarUrl { get; set; }
    
    
    public PersonalInfo? PersonalInfo { get; set; }
    
    public ICollection<UserActionLog> ActionLogs { get; set; } = new List<UserActionLog>();
    
    public ActivityTrackingSettings? ActivityTrackingSettings { get; set; }
    
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    
    public ICollection<WorkSpace> CreatedWorkspaces { get; set; } = new List<WorkSpace>();
    public ICollection<WorkSpace> DeletedWorkspaces { get; set; } = new List<WorkSpace>();
    
    public ICollection<WorkSpaceParticipant> WorkspacesParticipating { get; set; } = new List<WorkSpaceParticipant>();
    
    public ICollection<Board> CreatedBoards { get; set; } = new List<Board>();
    public ICollection<Board> DeletedBoards { get; set; } = new List<Board>();
    
    public ICollection<BoardParticipant> BoardParticipants { get; set; } = new List<BoardParticipant>();
    
    public ICollection<BoardList> CreatedLists { get; set; } = new List<BoardList>();
    public ICollection<BoardList> DeletedLists { get; set; } = new List<BoardList>();
    
    public ICollection<Card> CreatedCards { get; set; } = new List<Card>();
    public ICollection<Card> DeletedCards { get; set; } = new List<Card>();
    
    public ICollection<CardParticipant> CardParticipants { get; set; } = new List<CardParticipant>();
    
    public ICollection<Comment> CreatedComments { get; set; } = new List<Comment>();
    public ICollection<Comment> DeletedComments { get; set; } = new List<Comment>();
    
    public ICollection<CardAttachment> UploadedAttachments { get; set; } = new List<CardAttachment>();

}
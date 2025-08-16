using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public class BoardList : ILogEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public Board Board { get; set; } = null!;

    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = null!;
    public DateTime CreatingTimestamp { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public string? DeletedByUserId { get; set; }
    [ForeignKey(nameof(DeletedByUserId))]
    public UserProfile? DeletedByUser { get; set; }
    
    public ICollection<Card> Cards { get; set; } = new List<Card>();
    public EntityTargetType GetEntityType()
    {
        return EntityTargetType.BoardList;
    }

    public string GetName()
    {
        return Title;
    }

    public string GetId()
    {
        return Id.ToString();
    }

    public string GetSettingsLink()
    {
        return $"Lists/{GetId()}/Settings";
    }

    public string GetDescriptionName()
    {
        return "список";
    }
}
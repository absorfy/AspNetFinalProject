using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class CardParticipant
{
    public Guid CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public Card Card { get; set; } = null!;

    public string UserProfileId { get; set; } = null!;
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile UserProfile { get; set; } = null!;

    public DateTime JoiningTimestamp { get; set; } = DateTime.UtcNow;
}
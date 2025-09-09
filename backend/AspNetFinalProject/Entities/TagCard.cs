using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class TagCard
{
    public Guid TagId { get; set; }
    [ForeignKey(nameof(TagId))]
    public Tag Tag { get; set; } = null!;

    public Guid CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public Card Card { get; set; } = null!;
}
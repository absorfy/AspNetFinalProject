using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetFinalProject.Entities;

public class CardAttachment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public Card Card { get; set; } = null!;

    [Required]
    public string AuthorId { get; set; } = null!;
    [ForeignKey(nameof(AuthorId))]
    public UserProfile Author { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = null!; // Оригінальна назва файлу

    [Required]
    [MaxLength(255)]
    public string StoredFileName { get; set; } = null!; // Назва на сервері / GUID

    [MaxLength(100)]
    public string? ContentType { get; set; } // MIME type (image/png тощо)

    public long Size { get; set; } // у байтах

    [Required]
    [MaxLength(2048)]
    public string FilePath { get; set; } = null!; // Повний шлях або URL

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
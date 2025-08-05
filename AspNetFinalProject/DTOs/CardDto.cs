using System.ComponentModel.DataAnnotations;

namespace AspNetFinalProject.DTOs;

public class CardDto
{
    public int Id { get; set; }
    public int BoardListId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public DateTime? Deadline { get; set; }

    public string AuthorName { get; set; }
    public int ParticipantsCount { get; set; }
    public int CommentsCount { get; set; }
}

public class CreateCardDto
{
    [Required]
    public int BoardListId { get; set; }
    
    [Required]
    public string AuthorId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }

    public DateTime? Deadline { get; set; }
}

public class UpdateCardDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }

    public DateTime? Deadline { get; set; }
}
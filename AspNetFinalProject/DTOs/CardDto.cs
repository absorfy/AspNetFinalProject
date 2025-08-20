using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Common;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class CardDto
{
    public string Id { get; set; }
    public string BoardListId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public DateTime? Deadline { get; set; }

    public ParticipantRole? UserBoardRole { get; set; }
    
    public string AuthorName { get; set; }
    public int ParticipantsCount { get; set; }
    public int CommentsCount { get; set; }
}

public class CreateCardDto
{
    [Required]
    public string BoardListId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }

    public DateTime? Deadline { get; set; }
}

public class UpdateCardDto : ILogUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Color { get; set; }

    public DateTime? Deadline { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string BoardListTitle { get; set; }
}
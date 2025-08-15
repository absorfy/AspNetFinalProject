using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class BoardDto
{
    public string Id { get; set; }
    public string WorkSpaceId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Visibility { get; set; }
    
    public string AuthorName { get; set; }
    public int ParticipantsCount { get; set; }
    public ICollection<string> ListsIds { get; set; } = new List<string>();
}

public class CreateBoardDto
{
    [Required]
    public string WorkSpaceId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int Visibility { get; set; } = (int)BoardVisibility.Private;
}

public class UpdateBoardDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int Visibility { get; set; }
}
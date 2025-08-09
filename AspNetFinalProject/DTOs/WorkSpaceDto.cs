using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class WorkSpaceDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public WorkSpaceVisibility Visibility { get; set; }
    public DateTime CreatingTimestamp { get; set; }
    
    public string AuthorName { get; set; }
    public bool IsSubscribed { get; set; }
    
    public int BoardsCount { get; set; }
    public List<string> ParticipantIds { get; set; }
    //public List<string> JoinRequestIds { get; set; }
}

public class CreateWorkSpaceDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int Visibility { get; set; } = (int)WorkSpaceVisibility.Private;
}

public class UpdateWorkSpaceDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int Visibility { get; set; }
}
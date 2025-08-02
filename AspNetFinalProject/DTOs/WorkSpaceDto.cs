using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class WorkSpaceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public WorkSpaceVisibility Visibility { get; set; }
    
    public string AuthorName { get; set; }
    public int ParticipantsCount { get; set; }
}

public class CreateWorkspaceDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public WorkSpaceVisibility Visibility { get; set; } = WorkSpaceVisibility.Private;
}

public class UpdateWorkspaceDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public WorkSpaceVisibility Visibility { get; set; }
}
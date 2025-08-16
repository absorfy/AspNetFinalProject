using System.ComponentModel.DataAnnotations;
using AspNetFinalProject.Common;

namespace AspNetFinalProject.DTOs;

public class BoardListDto
{
    public string Id { get; set; }
    public string BoardId { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public ICollection<string> CardsIds { get; set; } = new List<string>();
}

public class CreateBoardListDto
{
    [Required]
    public string BoardId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
}

public class UpdateBoardListDto : ILogUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
}
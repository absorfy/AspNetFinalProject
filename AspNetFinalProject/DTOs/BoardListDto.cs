using System.ComponentModel.DataAnnotations;

namespace AspNetFinalProject.DTOs;

public class BoardListDto
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public int CardsCount { get; set; }
}

public class CreateBoardListDto
{
    [Required]
    public int BoardId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
}

public class UpdateBoardListDto
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
}
using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.BoardList.api;

[ApiController]
[Route("api/lists")]
[Authorize]
public class BoardListApiController : ControllerBase
{
    private readonly IBoardListService _boardListService;

    public BoardListApiController(IBoardListService boardListService)
    {
        _boardListService = boardListService;
    }
    
    [HttpGet("board/{boardId}")]
    public async Task<ActionResult<IEnumerable<BoardListDto>>> GetListsByBoard(int boardId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var lists = await _boardListService.GetListsByBoardAsync(boardId, userId);

        var result = lists.Select(l => new BoardListDto
        {
            Id = l.Id,
            BoardId = l.BoardId,
            Title = l.Title,
            AuthorName = l.Author.Username ?? l.Author.IdentityUser.UserName ?? "Unknown",
            CardsCount = l.Cards.Count
        });

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardListDto>> CreateBoardList([FromBody] CreateBoardListDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var list = await _boardListService.CreateAsync(dto.BoardId, dto.Title, userId);

        return CreatedAtAction(nameof(GetListsByBoard), new { boardId = dto.BoardId }, new BoardListDto
        {
            Id = list.Id,
            BoardId = list.BoardId,
            Title = list.Title,
            AuthorName = list.Author.Username ?? list.Author.IdentityUser.UserName ?? "Unknown",
            CardsCount = list.Cards.Count
        });
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBoardList(int id, [FromBody] UpdateBoardListDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _boardListService.UpdateAsync(id, dto.Title);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBoardList(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _boardListService.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
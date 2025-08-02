using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.Board.api;

[ApiController]
[Route("api/boards")]
[Authorize]
public class BoardApiController : ControllerBase
{
    private readonly IBoardService _boardService;
    
    public BoardApiController(IBoardService boardService)
    {
        _boardService = boardService;
    }
    
    [HttpGet("workspace/{workspaceId}")]
    public async Task<ActionResult<IEnumerable<BoardDto>>> GetBoardsByWorkspace(int workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var boards = await _boardService.GetBoardsByWorkSpaceAsync(workspaceId, userId);

        var result = boards.Select(b => new BoardDto
        {
            Id = b.Id,
            WorkSpaceId = b.WorkSpaceId,
            Title = b.Title,
            Description = b.Description,
            Visibility = b.Visibility,
            AuthorName = b.Author.Username ?? b.Author.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = b.Participants.Count
        });

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] CreateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var board = await _boardService.CreateAsync(dto.WorkSpaceId, dto.Title, userId, dto.Description);

        return CreatedAtAction(nameof(GetBoardsByWorkspace), new { workspaceId = dto.WorkSpaceId }, new BoardDto
        {
            Id = board.Id,
            WorkSpaceId = board.WorkSpaceId,
            Title = board.Title,
            Description = board.Description,
            Visibility = board.Visibility,
            AuthorName = board.Author.Username ?? board.Author.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = board.Participants.Count
        });
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBoard(int id, [FromBody] UpdateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _boardService.UpdateAsync(id, dto.Title, dto.Description, dto.Visibility);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBoard(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _boardService.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
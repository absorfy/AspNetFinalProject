using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Mappers;
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
    private readonly ICurrentUserService _currentUserService;
    
    public BoardApiController(
        IBoardService boardService,
        ICurrentUserService currentUserService)
    {
        _boardService = boardService;
        _currentUserService = currentUserService;
    }
    
    [HttpGet("workspace/{workspaceId:guid}")]
    public async Task<ActionResult<IEnumerable<BoardDto>>> GetBoardsByWorkspace(Guid workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var boards = await _boardService.GetAllByWorkSpaceAsync(workspaceId, userId);
        var result = boards.Select(BoardMapper.CreateDto);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] CreateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var board = await _boardService.CreateAsync(userId, dto);

        return CreatedAtAction(
            nameof(GetBoardsByWorkspace), 
            new { workspaceId = dto.WorkSpaceId }, 
            BoardMapper.CreateDto(board)
            );
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateBoard(Guid id, [FromBody] UpdateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = _currentUserService.GetIdentityId();
        if(userId == null) return Unauthorized();
        
        var updated = await _boardService.UpdateAsync(id, dto, userId);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteBoard(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _boardService.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
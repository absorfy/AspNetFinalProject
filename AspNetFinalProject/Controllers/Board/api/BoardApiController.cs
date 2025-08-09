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
    private readonly IBoardService _service;
    
    public BoardApiController(IBoardService service)
    {
        _service = service;
    }
    
    [HttpGet("workspace/{workspaceId}")]
    public async Task<ActionResult<IEnumerable<BoardDto>>> GetBoardsByWorkspace(string workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var boards = await _service.GetBoardsByWorkSpaceAsync(Guid.Parse(workspaceId), userId);
        var result = boards.Select(BoardMapper.CreateDto);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] CreateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var board = await _service.CreateAsync(userId, dto);

        return CreatedAtAction(
            nameof(GetBoardsByWorkspace), 
            new { workspaceId = dto.WorkSpaceId }, 
            BoardMapper.CreateDto(board)
            );
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBoard(string id, [FromBody] UpdateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(Guid.Parse(id), dto);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBoard(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _service.DeleteAsync(Guid.Parse(id), userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
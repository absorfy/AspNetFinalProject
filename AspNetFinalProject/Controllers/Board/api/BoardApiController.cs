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
    public async Task<ActionResult<IEnumerable<BoardDto>>> GetBoardsByWorkspace(int workspaceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var boards = await _service.GetBoardsByWorkSpaceAsync(workspaceId, userId);
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
    public async Task<ActionResult> UpdateBoard(int id, [FromBody] UpdateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBoard(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _service.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
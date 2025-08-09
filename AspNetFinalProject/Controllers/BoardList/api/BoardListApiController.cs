using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.BoardList.api;

[ApiController]
[Route("api/lists")]
[Authorize]
public class BoardListApiController : ControllerBase
{
    private readonly IBoardListService _service;

    public BoardListApiController(IBoardListService service)
    {
        _service = service;
    }
    
    [HttpGet("board/{boardId:guid}")]
    public async Task<ActionResult<IEnumerable<BoardListDto>>> GetListsByBoard(Guid boardId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var lists = await _service.GetListsByBoardAsync(boardId, userId);

        var result = lists.Select(BoardListMapper.CreateDto);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardListDto>> CreateBoardList([FromBody] CreateBoardListDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var list = await _service.CreateAsync(userId, dto);

        return CreatedAtAction(nameof(GetListsByBoard), new { boardId = dto.BoardId }, BoardListMapper.CreateDto(list));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateBoardList(Guid id, [FromBody] UpdateBoardListDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteBoardList(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _service.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
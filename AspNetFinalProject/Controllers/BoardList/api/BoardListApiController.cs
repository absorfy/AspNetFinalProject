using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;
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
    private readonly ICurrentUserService _currentUserService;

    public BoardListApiController(IBoardListService service,
                                  ICurrentUserService currentUserService)
    {
        _service = service;
        _currentUserService = currentUserService;
    }
    
    [HttpGet("board/{boardId:guid}")]
    public async Task<ActionResult<IEnumerable<BoardListDto>>> GetListsByBoard(Guid boardId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var lists = await _service.GetListsByBoardAsync(boardId, userId);

        var boardRole = await _currentUserService.GetBoardRoleAsync(boardId);
        var result = lists.Select(l => BoardListMapper.CreateDto(l, boardRole));

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardListDto>> CreateBoardList([FromBody] CreateBoardListDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var boardId = Guid.Parse(dto.BoardId);
        
        var hasPermission = await _currentUserService.HasBoardRoleAsync(boardId, 
            ParticipantRole.Admin, ParticipantRole.Owner, ParticipantRole.Member);
        if(!hasPermission) return Forbid();
        
        var list = await _service.CreateAsync(userId, dto);

        var boardRole = await _currentUserService.GetBoardRoleAsync(boardId);
        return CreatedAtAction(nameof(GetListsByBoard), new { boardId = dto.BoardId }, BoardListMapper.CreateDto(list, boardRole));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateBoardList(Guid id, [FromBody] UpdateBoardListDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = _currentUserService.GetIdentityId();
        if(userId == null) return Unauthorized();
        
        var updated = await _service.UpdateAsync(id, dto, userId);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteBoardList(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        
        var list = await _service.GetByIdAsync(id);
        if (list == null) return NotFound();
        var hasPermission = await _currentUserService.HasBoardRoleAsync(list.BoardId,
            ParticipantRole.Admin, ParticipantRole.Member, ParticipantRole.Owner);
        
        if(!hasPermission) return Forbid();
        
        var deleted = await _service.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
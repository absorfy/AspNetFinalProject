using System.Security.Claims;
using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
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
    public async Task<ActionResult<PagedResult<BoardDto>>> GetBoardsByWorkspace(Guid workspaceId, [FromQuery] PagedRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var pagedBoards = await (await _boardService.GetAllByWorkSpaceAsync(workspaceId, userId, request))
            .MapAsync<BoardDto>(async b =>
            {
                var isSubscribed = await _boardService.IsSubscribedAsync(b.Id, userId);
                return BoardMapper.CreateDto(b, userId, isSubscribed);
            });

        return Ok(pagedBoards);
    }
    
    [HttpGet("workspace/none")]
    public async Task<ActionResult<PagedResult<BoardDto>>> GetBoardsWithoutWorkspace([FromQuery] PagedRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var pagedBoards = await (await _boardService.GetAllWithoutWorkSpaceAsync(userId, request))
            .MapAsync<BoardDto>(async b =>
            {
                var isSubscribed = await _boardService.IsSubscribedAsync(b.Id, userId);
                return BoardMapper.CreateDto(b, userId, isSubscribed);
            });

        return Ok(pagedBoards);
    }
    
    [HttpPost]
    public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] CreateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (!await _currentUserService.HasWorkspaceRoleAsync(Guid.Parse(dto.WorkSpaceId), 
                ParticipantRole.Member, ParticipantRole.Admin, ParticipantRole.Owner))
        {
            return Forbid();
        }
        
        var board = await _boardService.CreateAsync(userId, dto);

        return CreatedAtAction(
            nameof(GetBoardsByWorkspace), 
            new { workspaceId = dto.WorkSpaceId }, 
            BoardMapper.CreateDto(board, userId)
            );
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateBoard(Guid id, [FromBody] UpdateBoardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = _currentUserService.GetIdentityId();
        if(userId == null) return Unauthorized();

        var board = await _boardService.GetByIdAsync(id);
        if (board == null) return NotFound();
        
        if (!await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId, ParticipantRole.Owner, ParticipantRole.Admin) &&
            !await _currentUserService.HasBoardRoleAsync(board.Id, ParticipantRole.Owner, ParticipantRole.Admin))
        {
            return Forbid();
        }
        
        var updated = await _boardService.UpdateAsync(id, dto, userId);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpPost("{id:guid}/subscribe")]
    public async Task<ActionResult> SubscribeToBoard(Guid id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var board = await _boardService.GetByIdAsync(id);
        if(board == null) return NotFound();
        
        if (!await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId,
                ParticipantRole.Owner, ParticipantRole.Member, ParticipantRole.Admin, ParticipantRole.Viewer) &&
            !await _currentUserService.HasBoardRoleAsync(id,
                ParticipantRole.Owner, ParticipantRole.Member, ParticipantRole.Admin, ParticipantRole.Viewer))
        {
            return Forbid();
        }
        
        var subscribed = await _boardService.SubscribeAsync(id, user.IdentityId);
        if (!subscribed) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}/subscribe")]
    public async Task<ActionResult> UnsubscribeFromBoard(Guid id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        var unsubscribed = await _boardService.UnsubscribeAsync(id, user.IdentityId);
        if (!unsubscribed) return NotFound();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteBoard(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        if (!await _currentUserService.HasBoardRoleAsync(id, ParticipantRole.Admin, ParticipantRole.Owner))
        {
            return Forbid();
        }
        
        var deleted = await _boardService.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
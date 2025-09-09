using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
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
public class BoardParticipantApiController : ControllerBase
{
    private readonly IBoardService _boardService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBoardParticipantRepository _boardParticipantRepository;

    public record ParticipantActionRequest(string UserProfileId);

    public record ParticipantRoleRequest(ParticipantRole Role);
    
    public BoardParticipantApiController(IBoardService boardService,
        ICurrentUserService currentUserService,
        IBoardParticipantRepository boardParticipantRepository)
    {
        _boardService = boardService;
        _currentUserService = currentUserService;
        _boardParticipantRepository = boardParticipantRepository;
    }
    
    
    [HttpGet("{boardId:guid}/participants/search")]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetNewParticipants(Guid boardId, [FromQuery] string q, [FromQuery] int take = 20)
    {
        var board = await _boardService.GetByIdAsync(boardId);
        if (board == null) return NotFound();
        
        if (!await _currentUserService.HasBoardRoleAsync(boardId, ParticipantRole.Admin, ParticipantRole.Owner) &&
            !await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId, ParticipantRole.Admin, ParticipantRole.Owner))
        {
            return Forbid();
        }
        
        q = (q ?? "").Trim();
        if (q.Length < 3) return Ok(Array.Empty<UserProfileDto>());

        var searched = await _boardParticipantRepository.GetNonParticipantsAsync(boardId, q);
        
        var result = searched
            .Take(Math.Clamp(take, 1, 50))
            .Select(UserProfileMapper.CreateDto)
            .ToList();
            
        return result;
    }
    
    
    [HttpPost("{boardId:guid}/participants/{userProfileId}/role")]
    public async Task<ActionResult> ChangeBoardParticipantRole(Guid boardId, string userProfileId,
        [FromBody] ParticipantRoleRequest request)
    {
        var userId = _currentUserService.GetIdentityId();
        if (userId == null) return Unauthorized();
        
        var participant = await _boardParticipantRepository.GetAsync(boardId, userProfileId);
        if(participant is null) return NotFound();

        var isNotAllowed = await IsNotAllowed(boardId, participant);
        if (isNotAllowed != null) return isNotAllowed;
        
        var board = await _boardService.GetByIdAsync(boardId);
        if (board == null) return NotFound();
        
        participant.Role = request.Role;
        await _boardParticipantRepository.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("{boardId:guid}/participants")]
    public async Task<ActionResult<PagedResult<BoardParticipantDto>>> GetBoardParticipants(Guid boardId, [FromQuery] PagedRequest request)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        
        var board = await _boardService.GetByIdAsync(boardId);
        if (board == null) return NotFound();
        
        if (!await _currentUserService.HasBoardRoleAsync(boardId, ParticipantRole.Admin, ParticipantRole.Owner,
                ParticipantRole.Member) &&
            !await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId, ParticipantRole.Admin, ParticipantRole.Owner))
        {
            return Forbid();
        }

        var pagedParticipants = (await _boardParticipantRepository.GetByBoardIdAsync(boardId, request))
            .MapAsync<BoardParticipantDto>(async bp => BoardParticipantMapper.CreateDto(bp, 
                await IsNotAllowed(boardId, bp) == null));

        return Ok(pagedParticipants);
    }

    [HttpGet("roles")]
    public ActionResult<ParticipantRoleDto> GetParticipantRoles()
    {
        var roles = Enum.GetValues<ParticipantRole>()
            .Select(ParticipantRoleMapper.CreateDto);
        return Ok(roles);
    }

    private async Task<ActionResult?> IsNotAllowed(Guid boardId, BoardParticipant target)
    {
        var board = await _boardService.GetByIdAsync(boardId);
        if (board == null) return NotFound();
        
        var changerId = _currentUserService.GetIdentityId();
        
        if (target.UserProfileId == changerId && 
            await _currentUserService.HasBoardRoleAsync(boardId, ParticipantRole.Owner))
        {
            return Forbid();
        }

        if ((await _currentUserService.HasBoardRoleAsync(boardId, ParticipantRole.Owner, ParticipantRole.Admin) ||
             await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId, ParticipantRole.Admin, ParticipantRole.Owner))&&
            target.Role != ParticipantRole.Owner)
        {
            return null;
        }

        return Forbid();
    }
    
    [HttpDelete("{boardId:guid}/participants")]
    public async Task<ActionResult> RemoveParticipant(Guid boardId, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");
        
        var participant = await _boardParticipantRepository.GetAsync(boardId, req.UserProfileId);
        if(participant is null) return NotFound();
        
        var notAllowed = await IsNotAllowed(boardId, participant);
        if(notAllowed is not null) return notAllowed;

        await _boardParticipantRepository.RemoveAsync(boardId, req.UserProfileId);
        await _boardParticipantRepository.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPost("{id:guid}/participants")]
    public async Task<ActionResult<WorkSpaceParticipantDto>> AddNewParticipant(Guid id, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");

        
        var board = await _boardService.GetByIdAsync(id);
        if (board == null) return NotFound();

        if (!await _currentUserService.HasBoardRoleAsync(id, ParticipantRole.Admin, ParticipantRole.Owner) &&
            !await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId, ParticipantRole.Admin, ParticipantRole.Owner))
        {
            return Forbid();
        }

        // 3) Не додавати двічі
        if (await _boardParticipantRepository.IsAlreadyParticipant(id, req.UserProfileId))
            return Conflict("User is already a participant of this workspace.");

        // 4) Створення і збереження
        var newParticipant = new BoardParticipant
        {
            BoardId = id,
            UserProfileId = req.UserProfileId,
            Role = ParticipantRole.Viewer,
            JoiningTimestamp = DateTime.UtcNow
        };

        await _boardParticipantRepository.AddAsync(newParticipant);
        await _boardParticipantRepository.SaveChangesAsync();

        // 5) Повторно завантажити з навігаціями для мапера (щоб не отримати NRE)
        var createdFull = await _boardParticipantRepository.GetAsync(id, req.UserProfileId);
        if(createdFull == null) return NotFound();
        
        return Ok(BoardParticipantMapper.CreateDto(createdFull, true));
    }
}
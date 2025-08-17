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
    private readonly IBoardService _boardSpaceService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBoardParticipantRepository _participantRepository;

    public record ParticipantActionRequest(string UserProfileId);

    public record ParticipantRoleRequest(string Role);
    
    public BoardParticipantApiController(IBoardService boardSpaceService,
        ICurrentUserService currentUserService,
        IBoardParticipantRepository participantRepository)
    {
        _boardSpaceService = boardSpaceService;
        _currentUserService = currentUserService;
        _participantRepository = participantRepository;
    }
    
    
    [HttpGet("{boardId:guid}/participants/search")]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetNewParticipants(Guid boardId, [FromQuery] string q, [FromQuery] int take = 20)
    {
        q = (q ?? "").Trim();
        if (q.Length < 3) return Ok(Array.Empty<UserProfileDto>());

        var searched = await _participantRepository.GetNonParticipantsAsync(boardId, q);
        
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
        
        var changer = await _participantRepository.GetAsync(boardId, userId);
        if(changer is null) return NotFound();
        
        var participant = await _participantRepository.GetAsync(boardId, userProfileId);
        if(participant is null) return NotFound();
        
        var notAllowed = IsNotAllowed(changer, participant);
        if(notAllowed is not null) return notAllowed;
        
        if (Enum.TryParse(request.Role, out ParticipantRole parsed))
        {
            if(parsed == ParticipantRole.Owner) return Forbid();
            participant.Role = parsed;
            await _participantRepository.SaveChangesAsync();
            return NoContent();
        }
        
        return BadRequest(new
        {
            error = $"Invalid role value: {request.Role}",
            allowedValues = Enum.GetNames<ParticipantRole>()
        });
    }
    
    [HttpGet("{boardId:guid}/participants")]
    public async Task<ActionResult<PagedResult<BoardParticipantDto>>> GetBoardParticipants(Guid boardId, [FromQuery] PagedRequest request)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        
        var board = await _boardSpaceService.GetByIdAsync(boardId);
        if (board == null) return NotFound();
        
        var participant = await _participantRepository.GetAsync(boardId, user.IdentityId);
        if (participant == null) return Forbid();

        var pagedParticipants = (await _participantRepository.GetByBoardIdAsync(boardId, request))
            .Map<BoardParticipantDto>(bp => BoardParticipantMapper.CreateDto(bp, IsNotAllowed(participant, bp) == null));

        return Ok(pagedParticipants);
    }

    [HttpGet("roles")]
    public ActionResult<ParticipantRoleDto> GetParticipantRoles()
    {
        var roles = Enum.GetValues<ParticipantRole>()
            .Select(ParticipantRoleMapper.CreateDto);
        return Ok(roles);
    }

    private ActionResult? IsNotAllowed(BoardParticipant changer, BoardParticipant target)
    {
        if ((changer.UserProfileId == target.UserProfileId && changer.Role == ParticipantRole.Owner) || (int)target.Role < (int)changer.Role)
        {
            return Forbid();
        }
        return null;
    }
    
    [HttpDelete("{boardId:guid}/participants")]
    public async Task<ActionResult> RemoveParticipant(Guid boardId, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");

        var userId = _currentUserService.GetIdentityId();
        if (userId == null) return Unauthorized();
        
        var changer = await _participantRepository.GetAsync(boardId, userId);
        if(changer is null) return NotFound();
        
        var participant = await _participantRepository.GetAsync(boardId, req.UserProfileId);
        if(participant is null) return NotFound();
        
        var notAllowed = IsNotAllowed(changer, participant);
        if(notAllowed is not null) return notAllowed;

        await _participantRepository.RemoveAsync(boardId, req.UserProfileId);
        await _participantRepository.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPost("{id:guid}/participants")]
    public async Task<ActionResult<WorkSpaceParticipantDto>> AddNewParticipant(Guid id, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");

        // 1) Поточний користувач
        var me = await _currentUserService.GetUserProfileAsync();
        if (me is null) return Unauthorized();

        // 2) Перевірка, що workspace існує + перевірка прав (Owner/Admin)
        var board = await _boardSpaceService.GetByIdAsync(id);
        if (board is null) return NotFound();

        var myMembership = board.Participants.FirstOrDefault(p => p.UserProfileId == me.IdentityId);
        var amAllowed = myMembership?.Role is ParticipantRole.Owner or ParticipantRole.Admin;
        if (!amAllowed) return Forbid();

        // 3) Не додавати двічі
        if (await _participantRepository.IsAlreadyParticipant(id, req.UserProfileId))
            return Conflict("User is already a participant of this workspace.");

        // 4) Створення і збереження
        var newParticipant = new BoardParticipant
        {
            BoardId = id,
            UserProfileId = req.UserProfileId,
            Role = ParticipantRole.Viewer,
            JoiningTimestamp = DateTime.UtcNow
        };

        await _participantRepository.AddAsync(newParticipant);
        await _participantRepository.SaveChangesAsync();

        // 5) Повторно завантажити з навігаціями для мапера (щоб не отримати NRE)
        var createdFull = await _participantRepository.GetAsync(id, req.UserProfileId);
        if(createdFull == null) return NotFound();
        
        return Ok(BoardParticipantMapper.CreateDto(createdFull, true));
    }
}
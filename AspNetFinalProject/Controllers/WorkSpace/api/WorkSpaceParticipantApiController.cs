using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.WorkSpace.api;

[ApiController]
[Route("api/workspaces")]
[Authorize]
public class WorkSpaceParticipantApiController : ControllerBase
{
    private readonly IWorkSpaceService _workSpaceService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkSpaceParticipantRepository _participantRepository;

    public record ParticipantActionRequest(string UserProfileId);

    public record ParticipantRoleRequest(ParticipantRole Role);
    
    public WorkSpaceParticipantApiController(IWorkSpaceService workSpaceService,
        ICurrentUserService currentUserService,
        IWorkSpaceParticipantRepository participantRepository)
    {
        _workSpaceService = workSpaceService;
        _currentUserService = currentUserService;
        _participantRepository = participantRepository;
    }
    
    
    [HttpGet("{workspaceId:guid}/participants/search")]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetNewParticipants(Guid workspaceId, [FromQuery] string q, [FromQuery] int take = 20)
    {
        var userId = _currentUserService.GetIdentityId();
        if(userId == null) return Unauthorized();

        if (!await _currentUserService.HasWorkspaceRoleAsync(workspaceId,
                ParticipantRole.Owner, ParticipantRole.Admin))
        {
            return Forbid();
        }
        
        q = (q ?? "").Trim();
        if (q.Length < 3) return Ok(Array.Empty<UserProfileDto>());

        var searched = await _participantRepository.GetNonParticipantsAsync(workspaceId, q);
        
        var result = searched
            .Take(Math.Clamp(take, 1, 50))
            .Select(UserProfileMapper.CreateDto)
            .ToList();
            
        return result;
    }

    [HttpPost("{workspaceId:guid}/participants/{userProfileId}/role")]
    public async Task<ActionResult> ChangeWorkspaceParticipantRole(Guid workspaceId, string userProfileId,
        [FromBody] ParticipantRoleRequest request)
    {
        var userId = _currentUserService.GetIdentityId();
        if (userId == null) return Unauthorized();
        
        var changer = await _participantRepository.GetAsync(workspaceId, userId);
        if(changer is null) return NotFound();
        
        var participant = await _participantRepository.GetAsync(workspaceId, userProfileId);
        if(participant is null) return NotFound();
        
        var notAllowed = IsNotAllowed(changer, participant);
        if(notAllowed is not null) return notAllowed;
        
        if(request.Role == ParticipantRole.Owner || (int)request.Role < (int)changer.Role) return Forbid();
        participant.Role = request.Role;
        await _participantRepository.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("{workspaceId:guid}/participants")]
    public async Task<ActionResult<PagedResult<WorkSpaceParticipantDto>>> GetWorkspaceParticipants(Guid workspaceId, [FromQuery] PagedRequest request)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        
        var workspace = await _workSpaceService.GetByIdAsync(workspaceId);
        if (workspace == null) return NotFound();
        
        if (!await _currentUserService.HasWorkspaceRoleAsync(workspaceId, ParticipantRole.Owner, ParticipantRole.Admin, ParticipantRole.Member))
        {
            return Forbid();
        }
        
        var participant = await _participantRepository.GetAsync(workspaceId, user.IdentityId);
        if (participant == null) return Forbid();

        var pagedParticipants = (await _participantRepository.GetByWorkSpaceIdAsync(workspaceId, request))
            .Map<WorkSpaceParticipantDto>(wp => WorkSpaceParticipantMapper.CreateDto(wp, IsNotAllowed(participant, wp) == null));

        return Ok(pagedParticipants);
    }

    [HttpGet("roles")]
    public ActionResult<ParticipantRoleDto> GetParticipantRoles()
    {
        var roles = Enum.GetValues<ParticipantRole>()
            .Select(ParticipantRoleMapper.CreateDto);
        return Ok(roles);
    }

    private ActionResult? IsNotAllowed(WorkSpaceParticipant changer, WorkSpaceParticipant target)
    {
        if ((changer.UserProfileId == target.UserProfileId && changer.Role == ParticipantRole.Owner) || (int)target.Role < (int)changer.Role || 
            changer.Role != ParticipantRole.Admin && changer.Role != ParticipantRole.Owner)
        {
            return Forbid();
        }
        return null;
    }
    
    [HttpDelete("{workspaceId:guid}/participants")]
    public async Task<ActionResult> RemoveParticipant(Guid workspaceId, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");

        var userId = _currentUserService.GetIdentityId();
        if (userId == null) return Unauthorized();
        
        var changer = await _participantRepository.GetAsync(workspaceId, userId);
        if(changer is null) return NotFound();
        
        var participant = await _participantRepository.GetAsync(workspaceId, req.UserProfileId);
        if(participant is null) return NotFound();
        
        var notAllowed = IsNotAllowed(changer, participant);
        if(notAllowed is not null) return notAllowed;

        await _participantRepository.RemoveAsync(workspaceId, req.UserProfileId);
        await _participantRepository.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPost("{id:guid}/participants")]
    public async Task<ActionResult<WorkSpaceParticipantDto>> AddNewParticipant(Guid id, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");

        var userId = _currentUserService.GetIdentityId();
        if (userId == null) return Unauthorized();

        if (!await _currentUserService.HasWorkspaceRoleAsync(id, ParticipantRole.Owner, ParticipantRole.Admin))
        {
            return Forbid();
        }

        if (await _participantRepository.IsAlreadyParticipant(id, req.UserProfileId))
            return Conflict("User is already a participant of this workspace.");


        var newParticipant = new WorkSpaceParticipant
        {
            WorkSpaceId = id,
            UserProfileId = req.UserProfileId,
            Role = ParticipantRole.Viewer,
            JoiningTimestamp = DateTime.UtcNow
        };

        await _participantRepository.AddAsync(newParticipant);
        await _participantRepository.SaveChangesAsync();

        // 5) Повторно завантажити з навігаціями для мапера (щоб не отримати NRE)
        var createdFull = (await _participantRepository.GetByWorkSpaceIdAsync(id))
            .First(p => p.UserProfileId == req.UserProfileId);

        return Ok(WorkSpaceParticipantMapper.CreateDto(createdFull, true));
    }
}
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

    public record ParticipantRoleRequest(string Role);
    
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
        q = (q ?? "").Trim();
        if (q.Length < 3) return Ok(Array.Empty<UserProfileDto>());

        var byName = await _participantRepository.GetNonParticipantsByUserNameAsync(workspaceId, q);
        var byEmail = await _participantRepository.GetNonParticipantsByEmailAsync(workspaceId, q);
        
        var result = byName.Concat(byEmail)
            .DistinctBy(u => u.IdentityId)
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
        
        if (Enum.TryParse(request.Role, out WorkSpaceRole parsed))
        {
            if(parsed == WorkSpaceRole.Owner) return Forbid();
            participant.Role = parsed;
            await _participantRepository.SaveChangesAsync();
            return NoContent();
        }
        
        return BadRequest(new
        {
            error = $"Invalid role value: {request.Role}",
            allowedValues = Enum.GetNames<WorkSpaceRole>()
        });
    }
    
    [HttpGet("{id:guid}/participants")]
    public async Task<ActionResult<IEnumerable<WorkSpaceParticipantDto>>> GetWorkspacesParticipants(Guid id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        
        var workspace = await _workSpaceService.GetByIdAsync(id);
        if (workspace == null) return NotFound();
        
        var participant = await _participantRepository.GetAsync(id, user.IdentityId);
        if (participant == null) return NotFound();
        
        return Ok(workspace.Participants.Select(p => 
            WorkSpaceParticipantMapper.CreateDto(p, IsNotAllowed(participant, p) == null)));
    }

    [HttpGet("roles")]
    public ActionResult<WorkSpaceRoleDto> GetWorkSpaceRoles()
    {
        var roles = Enum.GetValues<WorkSpaceRole>()
            .Select(WorkSpaceRoleMapper.CreateDto);
        return Ok(roles);
    }

    private ActionResult? IsNotAllowed(WorkSpaceParticipant changer, WorkSpaceParticipant target)
    {
        if ((changer.UserProfileId == target.UserProfileId && changer.Role == WorkSpaceRole.Owner) || (int)target.Role < (int)changer.Role)
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

        // 1) Поточний користувач
        var me = await _currentUserService.GetUserProfileAsync();
        if (me is null) return Unauthorized();

        // 2) Перевірка, що workspace існує + перевірка прав (Owner/Admin)
        var ws = await _workSpaceService.GetByIdAsync(id);
        if (ws is null) return NotFound();

        var myMembership = ws.Participants.FirstOrDefault(p => p.UserProfileId == me.IdentityId);
        var amAllowed = myMembership?.Role is WorkSpaceRole.Owner or WorkSpaceRole.Admin;
        if (!amAllowed) return Forbid();

        // 3) Не додавати двічі
        if (await _participantRepository.IsAlreadyParticipant(id, req.UserProfileId))
            return Conflict("User is already a participant of this workspace.");

        // 4) Створення і збереження
        var newParticipant = new WorkSpaceParticipant
        {
            WorkSpaceId = id,
            UserProfileId = req.UserProfileId,
            Role = WorkSpaceRole.Viewer,         // дефолт
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
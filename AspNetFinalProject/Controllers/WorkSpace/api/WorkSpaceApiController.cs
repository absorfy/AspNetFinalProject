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
public class WorkSpaceApiController : ControllerBase
{
    private readonly IWorkSpaceService _workSpaceService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkSpaceParticipantRepository _participantRepository;

    public WorkSpaceApiController(IWorkSpaceService workSpaceService,
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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkSpaceDto>>> GetMyWorkspaces()
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var workspaces = await _workSpaceService.GetUserWorkSpacesAsync(user.IdentityId);

        var resultTasks = workspaces.Select(async ws =>
        {
            var isSubscribed = await _workSpaceService.IsSubscribedAsync(ws.Id, user.IdentityId);
            return WorkSpaceMapper.CreateDto(ws, isSubscribed);
        });

        var result = await Task.WhenAll(resultTasks);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<WorkSpaceDto>> CreateWorkspace([FromBody] CreateWorkSpaceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        var workspace = await _workSpaceService.CreateAsync(user.IdentityId, dto);
        return CreatedAtAction(nameof(GetMyWorkspaces), new { id = workspace.Id }, WorkSpaceMapper.CreateDto(workspace));
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateWorkspace(string id, [FromBody] UpdateWorkSpaceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _workSpaceService.UpdateAsync(Guid.Parse(id), dto);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpPost("{id:guid}/subscribe")]
    public async Task<ActionResult> SubscribeToWorkspace(Guid id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var subscribed = await _workSpaceService.SubscribeAsync(id, user.IdentityId);
        if (!subscribed) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}/subscribe")]
    public async Task<ActionResult> UnsubscribeFromWorkspace(Guid id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        var unsubscribed = await _workSpaceService.UnsubscribeAsync(id, user.IdentityId);
        if (!unsubscribed) return NotFound();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorkspace(string id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var deleted = await _workSpaceService.DeleteAsync(Guid.Parse(id), user.IdentityId);
        if (!deleted) return NotFound();

        return NoContent();
    }

    [HttpGet("{id}/participants")]
    public async Task<ActionResult<IEnumerable<WorkSpaceParticipantDto>>> GetWorkspacesParticipants(string id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        
        var workspace = await _workSpaceService.GetByIdAsync(Guid.Parse(id));
        if (workspace == null) return NotFound();
        
        return Ok(workspace.Participants.Select(WorkSpaceParticipantMapper.CreateDto));
    }

    [HttpGet("roles")]
    public ActionResult<WorkSpaceRoleDto> GetWorkSpaceRoles()
    {
        var roles = Enum.GetValues<WorkSpaceRole>()
            .Select(WorkSpaceRoleMapper.CreateDto);
        return Ok(roles);
    }
    
    [HttpDelete("{id:guid}/participants")]
    public async Task<ActionResult> RemoveParticipant(Guid id, [FromBody] ParticipantActionRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserProfileId))
            return BadRequest("userProfileId is required.");
        
        var me = await _currentUserService.GetUserProfileAsync();
        if (me is null) return Unauthorized();
        
        var ws = await _workSpaceService.GetByIdAsync(id);
        if (ws is null) return NotFound();
        
        var myMembership = ws.Participants.FirstOrDefault(p => p.UserProfileId == me.IdentityId);
        var amAllowed = myMembership?.Role is WorkSpaceRole.Owner or WorkSpaceRole.Admin;
        if (!amAllowed) return Forbid();

        if (!await _participantRepository.IsAlreadyParticipant(id, req.UserProfileId))
            return Conflict("User is not a participant of this workspace.");

        await _participantRepository.RemoveAsync(id, req.UserProfileId);
        await _participantRepository.SaveChangesAsync();
        
        return NoContent();
    }
    
    public record ParticipantActionRequest(string UserProfileId);

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

        return Ok(WorkSpaceParticipantMapper.CreateDto(createdFull));
    }
    
}
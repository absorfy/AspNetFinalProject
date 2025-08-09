using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Implementations;
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
    
    [HttpPost("{id}/subscribe")]
    public async Task<ActionResult> SubscribeToWorkspace(string id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var subscribed = await _workSpaceService.SubscribeAsync(Guid.Parse(id), user.IdentityId);
        if (!subscribed) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}/subscribe")]
    public async Task<ActionResult> UnsubscribeFromWorkspace(string id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        var unsubscribed = await _workSpaceService.UnsubscribeAsync(Guid.Parse(id), user.IdentityId);
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
}
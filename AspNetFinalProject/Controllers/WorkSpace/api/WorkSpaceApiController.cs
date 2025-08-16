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
public class WorkSpaceApiController : ControllerBase
{
    private readonly IWorkSpaceService _workSpaceService;
    private readonly ICurrentUserService _currentUserService;

    public WorkSpaceApiController(IWorkSpaceService workSpaceService,
                                  ICurrentUserService currentUserService)
    {
        _workSpaceService = workSpaceService;
        _currentUserService = currentUserService;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<WorkSpaceDto>>> GetMyWorkspaces([FromQuery] PagedRequest request)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var pagedWorkspaces = await (await _workSpaceService.GetUserWorkSpacesAsync(user.IdentityId, request))
            .MapAsync<WorkSpaceDto>(async ws => 
            {
                var isSubscribed = await _workSpaceService.IsSubscribedAsync(ws.Id, user.IdentityId);
                return WorkSpaceMapper.CreateDto(ws, isSubscribed);
            });
        
        return Ok(pagedWorkspaces);
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

        var userId = _currentUserService.GetIdentityId();
        if(userId == null) return Unauthorized();
        
        var updated = await _workSpaceService.UpdateAsync(Guid.Parse(id), dto, userId);
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
}
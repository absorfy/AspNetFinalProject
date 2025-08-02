using System.Security.Claims;
using AspNetFinalProject.DTOs;
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

    public WorkSpaceApiController(IWorkSpaceService workSpaceService,
                                  ICurrentUserService currentUserService)
    {
        _workSpaceService = workSpaceService;
        _currentUserService = currentUserService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkSpaceDto>>> GetMyWorkspaces()
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var workspaces = await _workSpaceService.GetUserWorkSpacesAsync(user.IdentityId);

        var result = workspaces.Select(w => new WorkSpaceDto
        {
            Id = w.Id,
            Title = w.Title,
            Description = w.Description,
            Visibility = w.Visibility,
            AuthorName = w.Author.Username ?? w.Author.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = w.Participants.Count
        });

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<WorkSpaceDto>> CreateWorkspace([FromBody] CreateWorkspaceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var workspace = await _workSpaceService.CreateAsync(dto.Title, user.IdentityId, dto.Description);

        return CreatedAtAction(nameof(GetMyWorkspaces), new { id = workspace.Id }, new WorkSpaceDto
        {
            Id = workspace.Id,
            Title = workspace.Title,
            Description = workspace.Description,
            Visibility = workspace.Visibility,
            AuthorName = workspace.Author.Username ?? workspace.Author.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = workspace.Participants.Count
        });
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateWorkspace(int id, [FromBody] UpdateWorkspaceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _workSpaceService.UpdateAsync(id, dto.Title, dto.Description, dto.Visibility);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorkspace(int id)
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();

        var deleted = await _workSpaceService.DeleteAsync(id, user.IdentityId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
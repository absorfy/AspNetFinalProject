using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Mappers;
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
    private readonly WorkSpaceMapper _mapper;

    public WorkSpaceApiController(IWorkSpaceService workSpaceService,
                                  ICurrentUserService currentUserService,
                                  WorkSpaceMapper mapper)
    {
        _workSpaceService = workSpaceService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkSpaceDto>>> GetMyWorkspaces()
    {
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        var workspaces = await _workSpaceService.GetUserWorkSpacesAsync(user.IdentityId);
        var result = workspaces.Select(_mapper.ToDto);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<WorkSpaceDto>> CreateWorkspace([FromBody] CreateWorkSpaceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _currentUserService.GetUserProfileAsync();
        if (user == null) return Unauthorized();
        var workspace = await _workSpaceService.CreateAsync(user.IdentityId, dto);
        return CreatedAtAction(nameof(GetMyWorkspaces), new { id = workspace.Id }, _mapper.ToDto(workspace));
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateWorkspace(int id, [FromBody] UpdateWorkSpaceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _workSpaceService.UpdateAsync(id, dto);
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
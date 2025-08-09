using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.WorkSpace;

[Authorize]
[Route("workspaces")]
public class WorkSpacesController : Controller
{
    private readonly IWorkSpaceService _workSpaceService;
    private readonly ICurrentUserService _currentUserService;
    
    public WorkSpacesController(IWorkSpaceService workSpaceService, 
                                ICurrentUserService currentUserService)
    {
        _workSpaceService = workSpaceService;
        _currentUserService = currentUserService;
    }
    
    [HttpGet("{id}/settings")]
    public async Task<IActionResult> Settings(string id)
    {
        var userId = _currentUserService.GetIdentityId();
        if (userId == null)
            return Unauthorized();
        
        var workspace = await _workSpaceService.GetByIdAsync(Guid.Parse(id));
        if (workspace == null)
            return NotFound();
        
        var isAuthor = workspace.AuthorId == userId;
        var isAdmin = workspace.Participants.Any(p => p.UserProfileId == userId && p.Role == WorkSpaceRole.Admin);
        if (!isAuthor && !isAdmin)
            return Forbid();
        
        return View(WorkSpaceMapper.CreateDto(workspace));
    }
}
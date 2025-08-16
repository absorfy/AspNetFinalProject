using AspNetFinalProject.Common;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.WorkSpace;

[Authorize]
[Route("Workspaces")]
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
    
    [HttpGet("{id:guid}/Settings")]
    public async Task<IActionResult> Settings(Guid id)
    {
        var userId = _currentUserService.GetIdentityId();
        if (userId == null)
            return Unauthorized();
        
        var workspace = await _workSpaceService.GetByIdAsync(id);
        if (workspace == null)
            return NotFound();
        
        var isParticipant = workspace.Participants.Any(p => p.UserProfileId == userId);
        if(!isParticipant) return Forbid();
        
        var isSubscribed = await _workSpaceService.IsSubscribedAsync(id, userId);
        
        return View(WorkSpaceMapper.CreateDto(workspace, isSubscribed));
    }
}
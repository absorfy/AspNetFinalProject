using System.Text;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.Logs.api;

[ApiController]
[Route("api/logs")]
[Authorize]
public class LogApiController : ControllerBase
{
    private readonly IUserActionLogService _userActionLogService;
    
    public LogApiController(IUserActionLogService userActionLogService)
    {
        _userActionLogService = userActionLogService;
    }


    public record EntityLinkRequest(int EntityType, Guid EntityId);
    
    [HttpGet("/entity-link")]
    public async Task<ActionResult<string>> GetLinkToEntity([FromQuery]EntityLinkRequest request)
    {
        var entityType = (EntityTargetType)request.EntityType;
        var link = await _userActionLogService.GetEntityLink(entityType, request.EntityId);
        if(link == null) return NotFound();
        return Ok(link);
    }
}
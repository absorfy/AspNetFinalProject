using AspNetFinalProject.Data;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Controllers.Notification.api;


[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationApiController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUser;

    public NotificationApiController(INotificationService notificationService, ICurrentUserService currentUser)
    {
        _notificationService = notificationService;
        _currentUser = currentUser;
    }

    // GET /api/notifications?onlyUnread=true&take=50
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] bool onlyUnread = false, [FromQuery] int take = 3)
    {
        var userProfileId = _currentUser.GetIdentityId();
        if (userProfileId == null) return Unauthorized();
        var notifications = await _notificationService.GetAllByUserId(userProfileId, onlyUnread, take);
        var result = notifications.Select(NotificationMapper.CreateDto);
        return Ok(result);
    }

    // POST /api/notifications/{id}/read
    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var userProfileId = _currentUser.GetIdentityId();
        if (userProfileId == null) return Unauthorized();
        await _notificationService.MarkAsRead(id, userProfileId);
        return NoContent();
    }
}
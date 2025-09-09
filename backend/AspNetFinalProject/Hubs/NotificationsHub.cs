using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AspNetFinalProject.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
    public Task JoinMe(string userId)
        => Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");

    public Task LeaveMe(string userId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
}
using System.Security.Claims;
using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public string? GetIdentityId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<UserProfile?> GetUserProfileAsync()
    {
        var identityId = GetIdentityId();
        if (identityId == null) return null;

        return await _context.UserProfiles
            .Include(u => u.IdentityUser)
            .FirstOrDefaultAsync(u => u.IdentityId == identityId);
    }
}
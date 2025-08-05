using System.Security.Claims;
using AspNetFinalProject.Data;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserProfileRepository _repository;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserProfileRepository repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public string? GetIdentityId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<UserProfile?> GetUserProfileAsync()
    {
        var identityId = GetIdentityId();
        if (identityId == null) return null;
        return await _repository.GetByIdentityId(identityId);
    }
    
    public async Task<bool> UpdateAsync(UpdateUserProfileDto updateDto)
    {
        var userProfile = await GetUserProfileAsync();
        if (userProfile == null) return false;
        UserProfileMapper.UpdateEntity(userProfile, updateDto);
        await _repository.SaveChangesAsync();
        return true;
    }
}
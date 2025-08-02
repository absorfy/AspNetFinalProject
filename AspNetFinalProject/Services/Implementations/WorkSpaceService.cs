using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class WorkSpaceService : IWorkSpaceService
{
    private readonly IWorkSpaceRepository _workSpaceRepository;
    private readonly IWorkSpaceParticipantRepository _participantRepository;

    public WorkSpaceService(IWorkSpaceRepository workSpaceRepository, IWorkSpaceParticipantRepository participantRepository)
    {
        _workSpaceRepository = workSpaceRepository;
        _participantRepository = participantRepository;
    }


    public async Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId)
    {
        return await _workSpaceRepository.GetUserWorkSpacesAsync(userId);
    }

    public async Task<WorkSpace?> GetByIdAsync(int id)
    {
        return await _workSpaceRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, string title, string? description, WorkSpaceVisibility visibility)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        workspace.Title = title;
        workspace.Description = description;
        workspace.Visibility = visibility;

        await _workSpaceRepository.SaveChangesAsync();
        return true;
    }

    public async Task<WorkSpace> CreateAsync(string title, string authorId, string? description = null)
    {
        var workspace = new WorkSpace
        {
            Title = title,
            AuthorId = authorId,
            Description = description,
            CreatingTimestamp = DateTime.UtcNow
        };

        await _workSpaceRepository.AddAsync(workspace);
        await _workSpaceRepository.SaveChangesAsync();

        var admin = new WorkSpaceParticipant
        {
            UserProfileId = authorId,
            WorkSpaceId = workspace.Id,
            Role = WorkSpaceRole.Admin,
            JoiningTimestamp = DateTime.UtcNow
        };
        await _participantRepository.AddAsync(admin);
        await _participantRepository.SaveChangesAsync();
        
        return workspace;
    }

    public async Task<bool> DeleteAsync(int id, string deletedByUserId)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        
        workspace.DeletedByUserId = deletedByUserId;
        await _workSpaceRepository.DeleteAsync(workspace);
        await _workSpaceRepository.SaveChangesAsync();

        return true;
    }
}
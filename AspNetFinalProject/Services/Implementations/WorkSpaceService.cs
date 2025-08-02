using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class WorkSpaceService : IWorkSpaceService
{
    private readonly IWorkSpaceRepository _repository;

    public WorkSpaceService(IWorkSpaceRepository repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId)
    {
        return await _repository.GetUserWorkSpacesAsync(userId);
    }

    public async Task<WorkSpace?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, string title, string? description, WorkSpaceVisibility visibility)
    {
        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null) return false;

        workspace.Title = title;
        workspace.Description = description;
        workspace.Visibility = visibility;

        await _repository.SaveChangesAsync();
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

        await _repository.AddAsync(workspace);
        await _repository.SaveChangesAsync();
        
        return workspace;
    }

    public async Task<bool> DeleteAsync(int id, string deletedByUserId)
    {
        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null) return false;
        
        workspace.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(workspace);
        await _repository.SaveChangesAsync();

        return true;
    }
}
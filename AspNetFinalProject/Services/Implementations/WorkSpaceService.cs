using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class WorkSpaceService : IWorkSpaceService
{
    private readonly IWorkSpaceRepository _workSpaceRepository;
    private readonly IWorkSpaceParticipantRepository _participantRepository;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IWorkSpaceParticipantRepository _workSpaceParticipantRepository;

    public WorkSpaceService(IWorkSpaceRepository workSpaceRepository, 
                            IWorkSpaceParticipantRepository participantRepository, 
                            ISubscriptionService subscriptionService,
                            IWorkSpaceParticipantRepository workSpaceParticipantRepository)
    {
        _workSpaceRepository = workSpaceRepository;
        _participantRepository = participantRepository;
        _subscriptionService = subscriptionService;
        _workSpaceParticipantRepository = workSpaceParticipantRepository;
    }


    public async Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId)
    {
        return await _workSpaceRepository.GetUserWorkSpacesAsync(userId);
    }

    public async Task<WorkSpace?> GetByIdAsync(Guid id)
    {
        return await _workSpaceRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateWorkSpaceDto dto)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        WorkSpaceMapper.UpdateEntity(workspace, dto);
        await _workSpaceRepository.SaveChangesAsync();
        return true;
    }

    public async Task<WorkSpace> CreateAsync(string authorId, CreateWorkSpaceDto dto)
    {
        var workspace = WorkSpaceMapper.CreateEntity(authorId, dto);
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

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        
        workspace.DeletedByUserId = deletedByUserId;
        await _workSpaceRepository.DeleteAsync(workspace);
        await _workSpaceRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SubscribeAsync(Guid id, string userId)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        return await _subscriptionService.SubscribeAsync(userId, EntityTargetType.Workspace, id.ToString());
    }

    public async Task<bool> UnsubscribeAsync(Guid id, string userId)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        return await _subscriptionService.UnsubscribeAsync(userId, EntityTargetType.Workspace, id.ToString());
    }

    public Task<bool> IsSubscribedAsync(Guid id, string userId)
    {
        return _subscriptionService.IsSubscribedAsync(userId, EntityTargetType.Workspace, id.ToString());
    }
}
using AspNetFinalProject.Common;
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
    private readonly IBoardService _boardService;
    private readonly IWorkSpaceParticipantRepository _participantRepository;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ActionLogger _actionLogger;

    public WorkSpaceService(IWorkSpaceRepository workSpaceRepository, 
                            IWorkSpaceParticipantRepository participantRepository, 
                            ISubscriptionService subscriptionService,
                            ActionLogger actionLogger, IBoardService boardService)
    {
        _workSpaceRepository = workSpaceRepository;
        _participantRepository = participantRepository;
        _subscriptionService = subscriptionService;
        _actionLogger = actionLogger;
        _boardService = boardService;
    }

    
    
    public async Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId)
    {
        return await _workSpaceRepository.GetUserWorkSpacesAsync(userId);
    }
    
    public async Task<PagedResult<WorkSpace>> GetUserWorkSpacesAsync(string userId, PagedRequest request)
    {
        return await _workSpaceRepository.GetUserWorkSpacesAsync(userId, request);
    }

    public async Task<WorkSpace?> GetByIdAsync(Guid id)
    {
        return await _workSpaceRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateWorkSpaceDto dto, string updatedByUserId)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        var updateLogs = _actionLogger.CompareUpdateDtos(WorkSpaceMapper.CreateUpdateDto(workspace), dto).ToArray();
        if (updateLogs.Length == 0) return false;
        
        WorkSpaceMapper.UpdateEntity(workspace, dto);
        await _workSpaceRepository.SaveChangesAsync();

        await _actionLogger.LogAndNotifyAsync(updatedByUserId, workspace, UserActionType.Update, updateLogs);
        return true;
    }

    public async Task<WorkSpace> CreateAsync(string authorId, CreateWorkSpaceDto dto)
    {
        var workspace = WorkSpaceMapper.CreateEntity(authorId, dto);
        await _workSpaceRepository.AddAsync(workspace);
        await _workSpaceRepository.SaveChangesAsync();

        var owner = new WorkSpaceParticipant
        {
            UserProfileId = authorId,
            WorkSpaceId = workspace.Id,
            Role = ParticipantRole.Owner,
            JoiningTimestamp = DateTime.UtcNow
        };
        
        await _participantRepository.AddAsync(owner);
        await _participantRepository.SaveChangesAsync();
        
        await _actionLogger.LogAndNotifyAsync(authorId, workspace, UserActionType.Create);
        return workspace;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId, bool notify = true)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        
        workspace.DeletedByUserId = deletedByUserId;
        await _workSpaceRepository.DeleteAsync(workspace);
        await _workSpaceRepository.SaveChangesAsync();

        foreach (var workspaceBoard in workspace.Boards)
        {
            await _boardService.DeleteAsync(workspaceBoard.Id, deletedByUserId, false);
        }
        
        if(notify)
            await _actionLogger.LogAndNotifyAsync(deletedByUserId, workspace, UserActionType.Delete);
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
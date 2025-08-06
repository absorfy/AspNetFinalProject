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
    private readonly WorkSpaceMapper _mapper;

    public WorkSpaceService(IWorkSpaceRepository workSpaceRepository, 
                            IWorkSpaceParticipantRepository participantRepository, 
                            WorkSpaceMapper mapper)
    {
        _workSpaceRepository = workSpaceRepository;
        _participantRepository = participantRepository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId)
    {
        return await _workSpaceRepository.GetUserWorkSpacesAsync(userId);
    }

    public async Task<WorkSpace?> GetByIdAsync(int id)
    {
        return await _workSpaceRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, UpdateWorkSpaceDto dto)
    {
        var workspace = await _workSpaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        _mapper.UpdateEntity(workspace, dto);
        await _workSpaceRepository.SaveChangesAsync();
        return true;
    }

    public async Task<WorkSpace> CreateAsync(string authorId, CreateWorkSpaceDto dto)
    {
        var workspace = _mapper.CreateEntity(authorId, dto);
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
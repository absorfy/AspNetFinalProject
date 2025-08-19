using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Implementations;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.Board;

[Authorize]
[Route("Boards")]
public class BoardsController : Controller
{
    private readonly IBoardService _boardService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkSpaceParticipantRepository _workSpaceParticipantRepository;
    
    public BoardsController(IBoardService boardService, 
                            ICurrentUserService currentUserService,
                            IWorkSpaceParticipantRepository workSpaceParticipantRepository)
    {
        _boardService = boardService;
        _currentUserService = currentUserService;
        _workSpaceParticipantRepository = workSpaceParticipantRepository;
    }
    
    [HttpGet("{id}/Dashboard")]
    public async Task<IActionResult> Dashboard(string id)
    {
        var userId = _currentUserService.GetIdentityId();
        if(userId == null)
            return Unauthorized();
        
        var board = await _boardService.GetByIdAsync(Guid.Parse(id));
        if(board == null)
            return NotFound();

        var workSpaceParticipant = await _workSpaceParticipantRepository.GetAsync(board.WorkSpaceId, userId);


        if (board.Visibility == BoardVisibility.Public ||
            board.Visibility == BoardVisibility.Workspace && workSpaceParticipant != null ||
            board.Visibility == BoardVisibility.Private && (workSpaceParticipant is
                                                            {
                                                                Role: ParticipantRole.Admin or ParticipantRole.Owner
                                                            } ||
                                                            board.Participants.Any(p => p.UserProfileId == userId)))
        {
            return View(BoardMapper.CreateDto(board));
        }
        return Forbid();
    }

    [HttpGet("{id:guid}/Settings")]
    public async Task<IActionResult> Settings(Guid id)
    {
        var userId = _currentUserService.GetIdentityId();
        if(userId == null)
            return Unauthorized();
        
        var board = await _boardService.GetByIdAsync(id);
        if (board == null)
            return NotFound();


        if (!await _currentUserService.HasWorkspaceRoleAsync(board.WorkSpaceId, ParticipantRole.Admin,
                ParticipantRole.Owner) &&
            !await _currentUserService.HasBoardRoleAsync(board.Id, ParticipantRole.Owner, ParticipantRole.Admin,
                ParticipantRole.Member))
        {
            return Forbid();
        }

        var isSubscribed = await _boardService.IsSubscribedAsync(id, userId);
        
        return View(BoardMapper.CreateDto(board, isSubscribed));
    }
}
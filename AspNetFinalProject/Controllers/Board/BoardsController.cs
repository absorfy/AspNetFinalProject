using AspNetFinalProject.Mappers;
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
    
    public BoardsController(IBoardService boardService, 
                            ICurrentUserService currentUserService)
    {
        _boardService = boardService;
        _currentUserService = currentUserService;
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
        
        var isAuthor = board.AuthorId == userId;
        var isParticipant = board.Participants.Any(p => p.UserProfileId == userId);
        
        if(!isAuthor && !isParticipant)
            return Forbid();
        
        return View(BoardMapper.CreateDto(board));
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
        
        var isParticipant = board.Participants.Any(p => p.UserProfileId == userId);
        if(!isParticipant) return Forbid();

        var isSubscribed = await _boardService.IsSubscribedAsync(id, userId);
        
        return View(BoardMapper.CreateDto(board, isSubscribed));
    }
}
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Implementations;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.Board;

[Authorize]
public class BoardsController : Controller
{
    private readonly IBoardService _boardService;
    private readonly BoardMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    
    public BoardsController(IBoardService boardService, 
                            BoardMapper mapper,
                            ICurrentUserService currentUserService)
    {
        _boardService = boardService;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<IActionResult> Dashboard(int id)
    {
        var userId = _currentUserService.GetIdentityId();
        if(userId == null)
            return Unauthorized();
        
        var board = await _boardService.GetByIdAsync(id);
        if(board == null)
            return NotFound();
        
        var isAuthor = board.AuthorId == userId;
        var isParticipant = board.Participants.Any(p => p.UserProfileId == userId);
        
        if(!isAuthor && !isParticipant)
            return Forbid();
        
        return View(_mapper.ToDto(board));
    }
}
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.Board;

[Authorize]
public class BoardsController : Controller
{
    private readonly IBoardService _boardService;
    private readonly BoardMapper _mapper;
    
    public BoardsController(IBoardService boardService, BoardMapper mapper)
    {
        _boardService = boardService;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> Dashboard(int id)
    {
        var board = await _boardService.GetByIdAsync(id);
        if(board == null)
            return NotFound();
        
        return View(_mapper.ToDto(board));
    }
}
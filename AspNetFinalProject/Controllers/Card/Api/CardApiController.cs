using System.Security.Claims;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFinalProject.Controllers.Card.Api;

[ApiController]
[Route("api/cards")]
[Authorize]
public class CardApiController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly CardMapper _mapper;

    public CardApiController(ICardService cardService, CardMapper mapper)
    {
        _cardService = cardService;
        _mapper = mapper;
    }
    
    [HttpGet("list/{boardListId}")]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetCardsByList(int boardListId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var cards = await _cardService.GetCardsByListAsync(boardListId, userId);
        var result = cards.Select(_mapper.ToDto);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<CardDto>> CreateCard([FromBody] CreateCardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var card = await _cardService.CreateAsync(userId, dto);

        return CreatedAtAction(nameof(GetCardsByList), new { boardListId = dto.BoardListId }, _mapper.ToDto(card));
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCard(int id, [FromBody] UpdateCardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _cardService.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCard(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _cardService.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
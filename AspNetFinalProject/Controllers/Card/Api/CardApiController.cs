using System.Security.Claims;
using AspNetFinalProject.Common;
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
    private readonly ICurrentUserService _currentUserService;
    
    public CardApiController(ICardService cardService,
                             ICurrentUserService currentUserService)
    {
        _cardService = cardService;
        _currentUserService = currentUserService;
    }
    
    [HttpGet("list/{boardListId:guid}")]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetCardsByList(Guid boardListId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        var cards = await _cardService.GetCardsByListAsync(boardListId, userId);
        var result = cards.Select(CardMapper.CreateDto);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<CardDto>> CreateCard([FromBody] CreateCardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var card = await _cardService.CreateAsync(userId, dto);

        return CreatedAtAction(nameof(GetCardsByList), new { boardListId = dto.BoardListId }, CardMapper.CreateDto(card));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCard(Guid id, [FromBody] UpdateCardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = _currentUserService.GetIdentityId();
        if(userId == null) return Unauthorized();
        
        var updated = await _cardService.UpdateAsync(id, dto, userId);
        if (!updated) return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCard(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var deleted = await _cardService.DeleteAsync(id, userId);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
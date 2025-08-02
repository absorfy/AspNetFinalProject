using System.Security.Claims;
using AspNetFinalProject.DTOs;
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

    public CardApiController(ICardService cardService)
    {
        _cardService = cardService;
    }
    
    [HttpGet("list/{boardListId}")]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetCardsByList(int boardListId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var cards = await _cardService.GetCardsByListAsync(boardListId, userId);

        var result = cards.Select(c => new CardDto
        {
            Id = c.Id,
            BoardListId = c.BoardListId,
            Title = c.Title,
            Description = c.Description,
            Color = c.Color,
            Deadline = c.Deadline,
            AuthorName = c.Author.Username ?? c.Author.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = c.Participants.Count,
            CommentsCount = c.Comments.Count
        });

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<CardDto>> CreateCard([FromBody] CreateCardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var card = await _cardService.CreateAsync(dto.BoardListId, dto.Title, userId, dto.Description, dto.Color, dto.Deadline);

        return CreatedAtAction(nameof(GetCardsByList), new { boardListId = dto.BoardListId }, new CardDto
        {
            Id = card.Id,
            BoardListId = card.BoardListId,
            Title = card.Title,
            Description = card.Description,
            Color = card.Color,
            Deadline = card.Deadline,
            AuthorName = card.Author.Username ?? card.Author.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = card.Participants.Count,
            CommentsCount = card.Comments.Count
        });
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCard(int id, [FromBody] UpdateCardDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _cardService.UpdateAsync(id, dto.Title, dto.Description, dto.Color, dto.Deadline);
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
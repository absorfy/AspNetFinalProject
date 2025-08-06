using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;

namespace AspNetFinalProject.Repositories.Implementations;

public class BoardParticipantRepository : IBoardParticipantRepository
{
    private readonly ApplicationDbContext _context;
    
    public BoardParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(BoardParticipant participant)
    {
        await _context.BoardParticipants.AddAsync(participant);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
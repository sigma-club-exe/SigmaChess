
using SigmaChess.Core.Abstractions;
using SigmaChess.Core.Models;

namespace SigmaChess.DataAccess.Repository;

public class GameSessionRepository : IGameSessionRepository
{
    private readonly AppDbContext _context;

    public GameSessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateGame(string gameId, string gameCreatorTgId)
    {
        var gameSession = new GameSession(gameId, gameCreatorTgId, DateTime.UtcNow);
        await _context.DbGameSession.AddAsync(gameSession);
        await _context.SaveChangesAsync();
    }

    public async Task AcceptGame(string gameId, string acceptedByPlayerTgId)
    {
        var gameSession = _context.DbGameSession.Single(u => u.GameId == gameId);
        gameSession.Player2TgId = acceptedByPlayerTgId;
        gameSession.StartedAt = DateTime.UtcNow;
        gameSession.Status = GameStatus.InProgress;
        await _context.SaveChangesAsync();
    }
    
}
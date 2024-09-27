using SigmaChess.Core.Abstractions;
using SigmaChess.Core.Models;

namespace SigmaChess.DataAccess.Repository;

public class GameSessionInfoRepository : IGameSessionInfoRepository
{
    private readonly AppDbContext _context;

    public GameSessionInfoRepository(AppDbContext context)
    {
        _context = context;
    }

    public string GetOpponentUserName(string gameId, string userId)
    {
        var currentGame = _context.DbGameSession.Single(u => u.GameId == gameId);
        var opponentTgId = currentGame.Player2TgId == userId ? currentGame.Player1TgId : currentGame.Player2TgId;
        var opponentUsername = _context.DbUserAuth.Single(u => u.TgId == opponentTgId).TgUsername;
        return opponentUsername;
    }

    public GameStatus GetStatus(string gameId)
    {
        var currentGame = _context.DbGameSession.Single(u => u.GameId == gameId);
        return currentGame.Status;
    }

    public async Task PatchStatus(string gameId, GameStatus gameStatus)
    {
        var currentGame = _context.DbGameSession.Single(u => u.GameId == gameId);
        currentGame.Status = gameStatus;
        await _context.SaveChangesAsync();
    }

    public bool GameExists(string gameId)
    {
        return _context.DbGameSession.Any(u => u.GameId == gameId);
    }
}
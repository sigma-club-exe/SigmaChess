using SigmaChess.Core.Models;

namespace SigmaChess.Core.Abstractions;

public interface IGameSessionInfoRepository
{
    string GetOpponentUserName(string gameId, string userId);
    
    GameStatus GetStatus(string gameId);
    
    Task PatchStatus(string gameId, GameStatus gameStatus);
    
    bool GameExists(string gameId);
}
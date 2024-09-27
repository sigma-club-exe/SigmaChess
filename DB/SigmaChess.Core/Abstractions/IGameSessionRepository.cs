namespace SigmaChess.Core.Abstractions;
using SigmaChess.Core.Models;

public interface IGameSessionRepository
{
     Task CreateGame(string gameId, string gameCreatorTgId);
     Task AcceptGame(string gameId, string acceptedByPlayerTgId);
}
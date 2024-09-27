using SigmaChess.Core.Models;

namespace SigmaChess.Controllers;
using Microsoft.AspNetCore.Mvc;
using SigmaChess.Core.Abstractions;

[ApiController]
[Route("api/v1/gameSessionInfo")]
public class GameSessionInfoController : ControllerBase
{
    private readonly IGameSessionInfoRepository _gameSessionInfoRepository;

    public GameSessionInfoController(IGameSessionInfoRepository gameSessionInfoRepository)
    {
        _gameSessionInfoRepository = gameSessionInfoRepository;
    }
    
    [HttpGet("opponent-username")]
    public Task<ActionResult<string>> GetOpponentUsername([FromQuery] string gameId,[FromQuery] string userId)
    {
        var opponentUsername = _gameSessionInfoRepository.GetOpponentUserName(gameId, userId);
        return Task.FromResult<ActionResult<string>>(opponentUsername);
    }
    
    [HttpGet("status")]
    public Task<ActionResult<GameStatus>> GetStatus([FromQuery] string gameId)
    {
       var gameStatus = _gameSessionInfoRepository.GetStatus(gameId);
       return Task.FromResult<ActionResult<GameStatus>>(gameStatus);
    }
    
    [HttpPatch("patch-status")]
    public Task<ActionResult<GameStatus>> PatchStatus([FromQuery] string gameId, [FromQuery] GameStatus gameStatus)
    {
        _gameSessionInfoRepository.PatchStatus(gameId, gameStatus);
        return Task.FromResult<ActionResult<GameStatus>>(gameStatus);
    }

    [HttpGet("exists")]
    public Task<ActionResult<bool>> GameExists([FromQuery] string gameId)
    {
        var gameExists = _gameSessionInfoRepository.GameExists(gameId);
        return Task.FromResult<ActionResult<bool>>(gameExists);
    }
}
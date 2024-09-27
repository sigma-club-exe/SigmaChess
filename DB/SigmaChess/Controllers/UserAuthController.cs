using SigmaChess.Contracts.Contracts;

namespace SigmaChess.Controllers;
using Microsoft.AspNetCore.Mvc;
using SigmaChess.Core.Abstractions;

[ApiController]
[Route("api/v1/auth")]
public class UserAuthController : ControllerBase
{
    private readonly IUserAuthRepository _userAuthRepository;

    public UserAuthController(IUserAuthRepository userAuthRepository)
    {
        _userAuthRepository = userAuthRepository;
    }

    [HttpPost("user")]
    public async Task<ActionResult> AuthUser([FromQuery] string tgId, [FromQuery] string tgUsername,[FromQuery] string avatar)
    {
        await _userAuthRepository.AuthUser(tgId, tgUsername, avatar);
        return CreatedAtAction(nameof(AuthUser), new { tgId }, null);
    }

    [HttpGet("exists")]
    public Task<ActionResult<bool>> UserExists([FromQuery] string tgId)
    {
        var userExists = _userAuthRepository.UserExists(tgId);
        return Task.FromResult<ActionResult<bool>>(userExists);
    }
    
    [HttpGet("avatar")]
    public async Task<ActionResult> SetUserAvatar([FromQuery] string tgId, [FromQuery] string avatar)
    {
         await _userAuthRepository.SetUserAvatar(tgId, avatar);
         return CreatedAtAction(nameof(SetUserAvatar), new { tgId }, null);
    }
}
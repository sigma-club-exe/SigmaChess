namespace SigmaChess.Core.Abstractions;

public interface IUserAuthRepository
{
    Task AuthUser(string tgId, string tgUsername, string avatar);

    Task SetUserAvatar(string tgId, string avatar);

    bool UserExists(string tgId);
}
namespace Logger;

public interface IChessLogger
{
    Task Log(string tag, string message);
}
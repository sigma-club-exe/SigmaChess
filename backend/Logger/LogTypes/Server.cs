namespace Logger;

public abstract record Server : LogType
{
    public sealed record Challenge : Server;

    public sealed record Resign : Server;

    public sealed record DrawAccepted : Server;

    public sealed record Create : Server;

    public sealed record Move : Server;

    public sealed record Checkmate : Server;
}
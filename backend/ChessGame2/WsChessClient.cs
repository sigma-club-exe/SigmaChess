using Fleck;

namespace ChessGame2;

public class WsChessClient
{
    
    public WsChessClient(IWebSocketConnection playerConnection)
    {
        PlayerConnection = playerConnection;
    }
    
    public WsChessClient()
    {
        // bot has no IWebSocketConnection
    }

    public IWebSocketConnection? PlayerConnection {get; set;}
    public char Color { get; set; }
}
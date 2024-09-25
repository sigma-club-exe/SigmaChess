namespace ChessGame2;
using System.Net.Http;
using System.Text.Json;

public class BotEnemy
{
    // request to https://stockfish.online/api/s/v2.php
    
    //fen: rn1q1rk1/pp2b1pp/2p2n2/3p1pB1/3P4/1QP2N2/PP1N1PPP/R4RK1 b - - 1 11

    public BotEnemy()
    {
        
    }
    
    private static readonly HttpClient Client = new HttpClient();

    private const string _url = "https://stockfish.online/api/s/v2.php";

    public async Task<string> AskForBestMove(string fen)
    {
        // Формируем URL для запроса с переданным FEN и глубиной 10
        string requestUrl = $"{_url}?fen={fen}&depth=10";
    
        // Выполняем GET-запрос
        var response = await Client.GetAsync(requestUrl);
    
        // Читаем ответ
        var responseString = await response.Content.ReadAsStringAsync();

        // Парсим JSON-ответ
        var jsonDocument = JsonDocument.Parse(responseString);

        // Извлекаем значение ключа "bestmove"
        string bestMove = jsonDocument.RootElement.GetProperty("bestmove").GetString();

        // "bestmove b7b6 ponder a1e1" — разделяем строку по пробелам
        string[] moveParts = bestMove.Split(' ');
        string move = moveParts[1]; // Извлекаем ход, например "b7b6"

        return move; // Возвращаем ход
    }


}
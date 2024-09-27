using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SigmaChess.Core.Models;

public class GameSession(string gameId, string player1TgId, DateTime createdAt)
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string GameId { get; set; } = gameId;
    
    public GameStatus Status { get; set; } = GameStatus.NotStarted;
    
    [StringLength(30)]
    public string Player1TgId { get; set; } = player1TgId;
    
    [StringLength(30)]
    public string? Player2TgId { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = createdAt;
    
    public DateTime? StartedAt { get; set; }

    public virtual UserAuth? Player1 { get; set; }
    public virtual UserAuth? Player2 { get; set; }
}
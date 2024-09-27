using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SigmaChess.Core.Models;

public class UserAuth(string tgId, string tgUsername, DateOnly authDate, string avatar)
{
    [Key]
    [StringLength(30)]
    public string TgId { get; set; } = tgId;

    public string TgUsername { get; set; } = tgUsername;

    public DateOnly AuthDate { get; set; } = authDate;

    public string Avatar { get; set; } = avatar;
}
using Microsoft.EntityFrameworkCore;
using SigmaChess.Core.Models;

namespace SigmaChess.DataAccess;

public class AppDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserAuth> DbUserAuth { get; set; }
    public DbSet<GameSession> DbGameSession { get; set; }
    
}
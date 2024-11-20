using DataAccess.Abstractions;
using DataAccess.Models;

namespace DataAccess.Repository;

public class LogRepository : ILogRepository
{
    
    private readonly AppDbContext _context;

    public LogRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddLog(string tag, string message)
    {
        var tempLog = new Log(tag, message);
        await _context.Logs.AddAsync(tempLog);
        await _context.SaveChangesAsync();
    }
}
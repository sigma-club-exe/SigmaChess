using DataAccess.Models;

namespace DataAccess.Abstractions;

public interface ILogRepository
{
    Task AddLog(string tag, string message);
}
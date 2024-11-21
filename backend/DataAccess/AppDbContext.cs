using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        private readonly string _connectionString;

        // Добавляем конструктор, принимающий строку подключения
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Переопределяем метод OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}
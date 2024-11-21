namespace DataAccess.Models;

public class Log
{
    public Log(string tag, string message)
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        Tag = tag;
        Message = message;
    }

    public Log()
    {
    }


    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
   
    public string Tag { get; set; }
   
    public string Message { get; set; }
}
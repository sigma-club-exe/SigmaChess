namespace DataAccess.Models;

public class Log
{
    public Log(string tag, string message)
    {
        Tag = tag;
        Message = message;
        Timestamp = DateTime.Now;
        Id = Guid.NewGuid();
    }

   
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
   
    public string Tag { get; set; }
   
    public string Message { get; set; }
}
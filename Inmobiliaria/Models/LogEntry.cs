namespace Inmobiliaria.Models
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
        public ApplicationUser user { get; set; }
        public Exception Exception { get; set; }
        public string TimestampString { get; set; }
        public DateTime Timestamp { get; set; }

        public LogEntry()
        {
            Timestamp = DateTime.Now;
            TimestampString = Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        
    }
}

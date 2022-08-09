namespace app.Models
{
    public class LogEntryViewModel
    {
        public List<LogEntry> Items { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

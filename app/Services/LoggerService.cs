using app.Data;
using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Services
{
    public interface ILoggerService
    {

        Task<bool> Log(LogLevel level, string message, ApplicationUser user);

        Task<List<LogEntry>> GetAllLogs();
        Task<List<LogEntry>> GetLogsByLevel(LogLevel level);
        Task<List<LogEntry>> GetLogsByUser(ApplicationUser user);

        //getLogsDateRange
        Task<List<LogEntry>> GetLogsByDateRange(DateTime startDate, DateTime endDate);
    }
    public class LoggerService : ILoggerService
    {
        private readonly ApplicationDbContext _context;

        public LoggerService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<LogEntry>> GetAllLogs()
        {
            return await _context.LogEntries.ToListAsync();
        }

        public async Task<List<LogEntry>> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.LogEntries
               .Where(x => x.Timestamp >= startDate && x.Timestamp <= endDate)
               .ToListAsync();
        }

        public async Task<List<LogEntry>> GetLogsByLevel(LogLevel level)
        {
            return await _context.LogEntries.Where(x => x.LogLevel == level).ToListAsync();
        }

        public async Task<List<LogEntry>> GetLogsByUser(ApplicationUser user)
        {
            var items = await _context.LogEntries
                .Where(x => x.user.Email == user.Email)
                .ToListAsync();
            return items;
        }

        //Create a LogEntry and save
        public async Task<bool> Log(LogLevel level, string message, ApplicationUser user) {
            var logEntry = new LogEntry
            {
                LogLevel = level,
                Message = message,
                user = user
            };
            _context.LogEntries.Add(logEntry);
            var result = await _context.SaveChangesAsync();
            return result == 1;

        }
    }
}

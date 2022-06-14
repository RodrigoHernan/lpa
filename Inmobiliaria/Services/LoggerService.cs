using Inmobiliaria.Data;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ApplicationDbContext _context;

        public LoggerService(ApplicationDbContext context)
        {
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
        public async Task<bool> Log(LogLevel level, string message)
        {
            var logEntry = new LogEntry
            {
                LogLevel = level,
                Message = message,
                user = new ApplicationUser()
            };
            _context.LogEntries.Add(logEntry);
            var result = await _context.SaveChangesAsync();
            return result == 1;

        }
    }
}

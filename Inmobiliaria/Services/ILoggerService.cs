using Inmobiliaria.Models;

namespace Inmobiliaria.Services
{
    public interface ILoggerService
    {

        Task<bool> Log(LogLevel level, string message);
        Task<bool> Log(LogLevel level, string message, Exception ex);

        Task<List<LogEntry>> GetAllLogs();
        Task<List<LogEntry>> GetLogsByLevel(LogLevel level);

        //getlogbyuser
        Task<List<LogEntry>> GetLogsByUser(ApplicationUser user);

        //getLogsDateRange
        Task<List<LogEntry>> GetLogsByDateRange(DateTime startDate, DateTime endDate);



    }
}

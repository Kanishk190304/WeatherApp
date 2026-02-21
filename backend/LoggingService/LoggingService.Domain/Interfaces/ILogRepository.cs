using LoggingService.Domain.Entities;

namespace LoggingService.Domain.Interfaces
{
    // Contract for log storage operations
    public interface ILogRepository
    {
        // Request logs
        Task CreateRequestLogAsync(RequestLog log);
        Task<IEnumerable<RequestLog>> GetRequestLogsAsync(int page, int pageSize);

        // Error logs
        Task CreateErrorLogAsync(ErrorLog log);
        Task<IEnumerable<ErrorLog>> GetErrorLogsAsync(int page, int pageSize);
    }
}
using LoggingService.Application.DTOs;
using LoggingService.Domain.Entities;

namespace LoggingService.Application.Interfaces
{
    // Contract for logging service operations
    public interface ILoggingService
    {
        Task LogRequestAsync(RequestLogDto logDto);
        Task LogErrorAsync(ErrorLogDto logDto);
        Task<IEnumerable<RequestLog>> GetRequestLogsAsync(int page, int pageSize);
        Task<IEnumerable<ErrorLog>> GetErrorLogsAsync(int page, int pageSize);
    }
}
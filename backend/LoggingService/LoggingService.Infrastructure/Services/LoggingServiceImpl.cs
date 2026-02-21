using LoggingService.Application.DTOs;
using LoggingService.Application.Interfaces;
using LoggingService.Domain.Entities;
using LoggingService.Domain.Interfaces;

namespace LoggingService.Infrastructure.Services
{
    // Singleton Pattern: Managed by DI container (AddSingleton in Program.cs)
    public class LoggingServiceImpl : ILoggingService
    {
        private readonly ILogRepository _logRepository;

        public LoggingServiceImpl(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        // Log API request
        public async Task LogRequestAsync(RequestLogDto logDto)
        {
            var log = new RequestLog
            {
                ServiceName = logDto.ServiceName,
                Endpoint = logDto.Endpoint,
                Method = logDto.Method,
                RequestBody = logDto.RequestBody,
                ResponseBody = logDto.ResponseBody,
                StatusCode = logDto.StatusCode,
                DurationMs = logDto.DurationMs,
                UserId = logDto.UserId,
                Timestamp = DateTime.UtcNow
            };

            await _logRepository.CreateRequestLogAsync(log);
        }

        // Log error
        public async Task LogErrorAsync(ErrorLogDto logDto)
        {
            var log = new ErrorLog
            {
                ServiceName = logDto.ServiceName,
                Endpoint = logDto.Endpoint,
                ErrorMessage = logDto.ErrorMessage,
                StackTrace = logDto.StackTrace,
                ExceptionType = logDto.ExceptionType,
                UserId = logDto.UserId,
                Timestamp = DateTime.UtcNow
            };

            await _logRepository.CreateErrorLogAsync(log);
        }

        // Get request logs
        public async Task<IEnumerable<RequestLog>> GetRequestLogsAsync(int page, int pageSize)
        {
            return await _logRepository.GetRequestLogsAsync(page, pageSize);
        }

        // Get error logs
        public async Task<IEnumerable<ErrorLog>> GetErrorLogsAsync(int page, int pageSize)
        {
            return await _logRepository.GetErrorLogsAsync(page, pageSize);
        }
    }
}
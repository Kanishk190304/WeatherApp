using LoggingService.Domain.Entities;
using LoggingService.Domain.Interfaces;
using LoggingService.Infrastructure.Data;
using MongoDB.Driver;

namespace LoggingService.Infrastructure.Repositories
{
    // Implementation of ILogRepository using MongoDB
    public class LogRepository : ILogRepository
    {
        private readonly MongoDbContext _context;

        public LogRepository(MongoDbContext context)
        {
            _context = context;
        }

        // Create request log
        public async Task CreateRequestLogAsync(RequestLog log)
        {
            await _context.RequestLogs.InsertOneAsync(log);
        }

        // Get request logs with pagination
        public async Task<IEnumerable<RequestLog>> GetRequestLogsAsync(int page, int pageSize)
        {
            return await _context.RequestLogs
                .Find(_ => true)
                .SortByDescending(x => x.Timestamp)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        // Create error log
        public async Task CreateErrorLogAsync(ErrorLog log)
        {
            await _context.ErrorLogs.InsertOneAsync(log);
        }

        // Get error logs with pagination
        public async Task<IEnumerable<ErrorLog>> GetErrorLogsAsync(int page, int pageSize)
        {
            return await _context.ErrorLogs
                .Find(_ => true)
                .SortByDescending(x => x.Timestamp)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }
    }
}
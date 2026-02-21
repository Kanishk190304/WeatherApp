using LoggingService.Application.DTOs;
using LoggingService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.API.Controllers
{
    // API controller for logging endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        public LogsController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // POST: api/logs/request
        [HttpPost("request")]
        public async Task<IActionResult> LogRequest([FromBody] RequestLogDto logDto)
        {
            await _loggingService.LogRequestAsync(logDto);
            return Ok(new { Message = "Request logged successfully" });
        }

        // POST: api/logs/error
        [HttpPost("error")]
        public async Task<IActionResult> LogError([FromBody] ErrorLogDto logDto)
        {
            await _loggingService.LogErrorAsync(logDto);
            return Ok(new { Message = "Error logged successfully" });
        }

        // GET: api/logs/requests?page=1&pageSize=10
        [HttpGet("requests")]
        public async Task<IActionResult> GetRequestLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = await _loggingService.GetRequestLogsAsync(page, pageSize);
            return Ok(logs);
        }

        // GET: api/logs/errors?page=1&pageSize=10
        [HttpGet("errors")]
        public async Task<IActionResult> GetErrorLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = await _loggingService.GetErrorLogsAsync(page, pageSize);
            return Ok(logs);
        }

        // GET: api/logs/test
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { Message = "Logging Service is running", Timestamp = DateTime.UtcNow });
        }
    }
}
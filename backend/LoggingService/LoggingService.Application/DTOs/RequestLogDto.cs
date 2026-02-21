namespace LoggingService.Application.DTOs
{
    // Data transfer object for creating request logs
    public class RequestLogDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public string ResponseBody { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
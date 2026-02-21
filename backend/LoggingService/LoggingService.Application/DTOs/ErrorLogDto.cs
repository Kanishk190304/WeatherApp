namespace LoggingService.Application.DTOs
{
    // Data transfer object for creating error logs
    public class ErrorLogDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string ExceptionType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
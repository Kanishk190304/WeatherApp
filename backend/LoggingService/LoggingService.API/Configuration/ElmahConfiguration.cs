using ElmahCore;
using ElmahCore.Mvc;

namespace LoggingService.API.Configuration
{
    /// <summary>
    /// ELMAH Configuration for Error Logging
    /// Configures centralized error logging for all exceptions
    /// </summary>
    public static class ElmahConfiguration
    {
        /// <summary>
        /// Adds ELMAH error logging services
        /// </summary>
        public static IServiceCollection AddElmahLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.Path = @"~/elmah";
                options.LogPath = "~/logs/elmah";
            });

            return services;
        }

        /// <summary>
        /// Uses ELMAH error logging middleware
        /// </summary>
        public static WebApplication UseElmahLogging(this WebApplication app)
        {
            app.UseElmahExceptionPage();
            app.UseElmah();

            return app;
        }
    }
}

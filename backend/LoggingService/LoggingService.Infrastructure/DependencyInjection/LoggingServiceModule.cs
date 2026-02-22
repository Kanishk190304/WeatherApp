using Autofac;
using LoggingService.Application.Interfaces;
using LoggingService.Domain.Interfaces;
using LoggingService.Infrastructure.Data;
using LoggingService.Infrastructure.Repositories;
using LoggingService.Infrastructure.Services;

namespace LoggingService.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Autofac module for registering all Logging Service dependencies
    /// Follows Onion Architecture with clear separation of concerns
    /// </summary>
    public class LoggingServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register MongoDB Context (Singleton for MongoDB connection)
            builder.RegisterType<MongoDbContext>()
                .SingleInstance();

            // Register Repositories (Infrastructure Layer)
            builder.RegisterType<LogRepository>()
                .As<ILogRepository>()
                .InstancePerLifetimeScope();

            // Register Application Services
            builder.RegisterType<LoggingServiceImpl>()
                .As<ILoggingService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}

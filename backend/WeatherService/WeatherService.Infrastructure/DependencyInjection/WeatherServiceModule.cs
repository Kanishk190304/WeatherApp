using Autofac;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Interfaces;
using WeatherService.Infrastructure.Caching;
using WeatherService.Infrastructure.ExternalApis;
using WeatherService.Infrastructure.Services;

namespace WeatherService.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Autofac module for registering all Weather Service dependencies
    /// Follows Onion Architecture with clear separation of concerns
    /// </summary>
    public class WeatherServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Factory Pattern (Weather Provider Factory)
            builder.RegisterType<WeatherProviderFactory>()
                .As<IWeatherProviderFactory>()
                .SingleInstance();

            // Register Weather Provider using Factory
            builder.Register<IWeatherProvider>(ctx =>
            {
                var factory = ctx.Resolve<IWeatherProviderFactory>();
                return factory.CreateProvider("openweathermap");
            })
            .InstancePerLifetimeScope();

            // Register Caching Services (Singleton Pattern)
            builder.RegisterType<RedisCacheServiceFactory>()
                .SingleInstance();

            builder.Register<ICacheService>(ctx =>
            {
                var factory = ctx.Resolve<RedisCacheServiceFactory>();
                return factory.Create();
            })
            .SingleInstance();

            // Register Application Services
            builder.RegisterType<WeatherDataService>()
                .As<IWeatherService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}

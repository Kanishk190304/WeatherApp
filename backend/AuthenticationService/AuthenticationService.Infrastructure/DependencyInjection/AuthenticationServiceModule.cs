using Autofac;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Interfaces;
using AuthenticationService.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Services;

namespace AuthenticationService.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Autofac module for registering all Authentication Service dependencies
    /// Follows Onion Architecture with clear separation of concerns
    /// </summary>
    public class AuthenticationServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Repositories (Infrastructure Layer)
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserAdoRepository>()
                .As<IUserAdoRepository>()
                .InstancePerLifetimeScope();

            // Register Application Services
            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JwtService>()
                .As<IJwtService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GoogleAuthService>()
                .As<IGoogleAuthService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}

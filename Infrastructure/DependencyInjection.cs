using Application.Common.Options;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtAuth(configuration);
        
        var connectionString = configuration.GetConnectionString("Postgres");

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o => o.MigrationsAssembly("Infrastructure"));
        });
        
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}
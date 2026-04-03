using System.Security.Claims;
using System.Text;
using Application.Common.Options;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt").Get<JwtSettings>();
        var secret = jwtSection?.SecretKey;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuers = new[] { jwtSection?.Issuer, jwtSection?.ServiceIssuer },
                    ValidAudiences = new[] { jwtSection?.Audience, jwtSection?.ServiceAudience },

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secret!)),

                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("OrganizationsRead", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireClaim("scope", "crm.organizations.read", "crm.organizations.write"));

            options.AddPolicy("OrganizationsWrite", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole(nameof(ERole.Admin))
                    .RequireClaim("scope", "crm.organizations.write"));
        });

        return services;
    }
}

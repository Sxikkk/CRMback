using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Options;
using Infrastructure.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Tests;

public class JwtTokenGeneratorTests
{
    private static JwtSettings CreateSettings() => new()
    {
        SecretKey = "super-secret-key-32-bytes-long-123",
        Issuer = "crm-api",
        Audience = "crm-client",
        ExpirationMinutes = 60
    };

    [Fact]
    public void GenerateToken_should_return_valid_jwt()
    {
        var settings = CreateSettings();
        var generator = new JwtTokenGenerator(Options.Create(settings));

        var token = generator.GenerateToken(Guid.NewGuid(), "jdoe", Domain.Enums.ERole.Admin);

        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = settings.Issuer,
            ValidAudience = settings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };

        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

        Assert.IsType<JwtSecurityToken>(validatedToken);
        Assert.NotNull(principal.Identity);
        Assert.True(principal.Identity!.IsAuthenticated);
    }
}


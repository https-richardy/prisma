// Kyon - Open Source Initiative
// Licensed under the MIT License

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kyon.Dolphin.Security.Extensions;

/// <summary>
/// Provides extensions for configuring JWT services.
/// </summary>
/// <remarks>
/// This class contains extension methods for adding JWT-related services to the
/// dependency injection container, allowing easy integration with authentication and
/// authorization in ASP.NET Core applications.
/// </remarks>
public static class JwtServiceExtensions
{
    /// <summary>
    /// Adds the JWT service to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the JWT service to.</param>
    /// <param name="configuration">The configuration providing access to application settings.</param>

    #pragma warning disable CS8604
    public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtService, JwtService>();
        var secretKey = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });
        services.AddAuthorization();
    }

    /// <summary>
    /// Adds the JWT service to the service collection with custom options.
    /// </summary>
    /// <param name="services">The service collection to add the JWT service to.</param>
    /// <param name="configuration">The configuration providing access to application settings.</param>
    /// <param name="configureOptions">An action to configure the JWT options.</param>
    public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration, Action<JwtOptions> configureOptions)
    {
        var options = new JwtOptions();
        var secretKey = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);

        configureOptions(options);

        services.AddScoped<IJwtService, JwtService>(provider => new JwtService(configuration, options));
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });
        services.AddAuthorization();
    }
}
#pragma warning restore CS8604
/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Prisma.Security.Extensions;

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
    /// Adds the JWT service to the service collection, configures authentication, and registers authorization.
    /// </summary>
    /// <param name="services">The service collection to add the JWT service to.</param>
    /// <param name="configuration">The configuration providing access to application settings.</param>
    /// <remarks>
    /// <para>
    /// The <c>AddJwtBearer</c> method is an extension method for <see cref="IServiceCollection"/>,
    /// designed to simplify the integration of the JWT service into the application's dependency injection container.
    /// </para>
    /// <para>
    /// It performs the following tasks:
    /// <list type="bullet">
    /// <item>
    /// <description>Registers the <see cref="IJwtService"/> interface with the implementation of <see cref="JwtService"/> in the dependency injection container.</description>
    /// </item>
    /// <item>
    /// <description>
    /// Retrieves the JWT secret key from the application settings using the provided <paramref name="configuration"/> and converts it to a byte array.
    /// </description>
    /// </item>
    /// <item>
    /// <description>Configures the authentication middleware with JWT Bearer authentication as the default scheme for both authentication and challenge.</description>
    /// </item>
    /// <item>
    /// <description>
    /// Configures the <see cref="TokenValidationParameters"/> for the JWT Bearer authentication, including options such as validating the issuer signing key,
    /// disabling issuer and audience validation, and setting the clock skew to allow for a time difference between the server and the client.
    /// </description>
    /// </item>
    /// <item>
    /// <description>Registers the authentication middleware with the service collection.</description>
    /// </item>
    /// <item>
    /// <description>Registers the authorization services with the service collection.</description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>

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
    /// <remarks>
    /// <para>
    /// The <c>AddJwtBearer</c> method is an extension method for <see cref="IServiceCollection"/>,
    /// designed to simplify the integration of the JWT service into the application's dependency injection container with custom options.
    /// </para>
    /// <para>
    /// It allows customization of the JWT service through the <paramref name="configureOptions"/> action, providing a flexible way
    /// to set properties such as the cryptographic key, security algorithm, and expiration time for the JWT.
    /// </para>
    /// <para>
    /// This method performs the following tasks:
    /// <list type="bullet">
    /// <item>
    /// <description>Creates an instance of <see cref="JwtOptions"/> to store and configure the options for the JWT service.</description>
    /// </item>
    /// <item>
    /// <description>Retrieves the JWT secret key from the application settings using the provided <paramref name="configuration"/> and converts it to a byte array.</description>
    /// </item>
    /// <item>
    /// <description>Invokes the <paramref name="configureOptions"/> action to allow customization of the JWT options.</description>
    /// </item>
    /// <item>
    /// <description>Registers the <see cref="IJwtService"/> interface with the implementation of <see cref="JwtService"/> in the dependency injection container,
    /// providing the configuration and options.</description>
    /// </item>
    /// <item>
    /// <description>Configures the authentication middleware with JWT Bearer authentication as the default scheme for both authentication and challenge.</description>
    /// </item>
    /// <item>
    /// <description>
    /// Configures the <see cref="TokenValidationParameters"/> for the JWT Bearer authentication, including options such as validating the issuer signing key,
    /// disabling issuer and audience validation, and setting the clock skew to allow for a time difference between the server and the client.
    /// </description>
    /// </item>
    /// <item>
    /// <description>Registers the authentication middleware with the service collection.</description>
    /// </item>
    /// <item>
    /// <description>Registers the authorization services with the service collection.</description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
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
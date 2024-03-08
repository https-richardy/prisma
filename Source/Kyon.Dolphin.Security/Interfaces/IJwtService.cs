// Kyon - Open Source Initiative
// Licensed under the MIT License

using System.Security.Claims;

namespace Kyon.Dolphin.Security;

/// <summary>
/// Interface for a service responsible for creating JSON Web Token (JWT) based on provided user information (claims identity).
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT based on the provided claims identity.
    /// </summary>
    /// <param name="claimsIdentity">User information and claims to be included in the JWT.</param>
    /// <returns>Generated JWT as a string.</returns>
    string GenerateToken(ClaimsIdentity claimsIdentity);
}
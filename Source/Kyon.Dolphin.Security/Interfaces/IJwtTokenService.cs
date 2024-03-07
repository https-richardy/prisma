using System.Security.Claims;

namespace Kyon.Dolphin.Security;

public interface IJwtTokenService
{
    string GenerateToken(ClaimsIdentity claimsIdentity);
}
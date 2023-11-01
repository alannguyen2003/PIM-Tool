using System.Security.Claims;
using PIMTool.Payload.Request.Authentication;

namespace PIMTool.Services;

public interface IAuthenticationService
{
    public Claim? GetRole(ClaimsPrincipal claimsPrincipal);
    public bool IsAccessible(string currentRole, string requireRole);
}

public class AuthenticationService : IAuthenticationService
{
    public Claim? GetRole(ClaimsPrincipal claimsPrincipal)
    {
        var role = claimsPrincipal.Claims.FirstOrDefault(x =>
            x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
        return role;
    }

    public bool IsAccessible(string currentRole, string requireRole)
    {
        Role current, require;
        Enum.TryParse(currentRole, out current);
        Enum.TryParse(requireRole, out require);
        if (current <= require) return true;
        return false;
    }
}
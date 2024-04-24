using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Extensions;

public static class IdentityResultExtensions
{
    public static string? GetResultErrors(this IdentityResult result)
    {
        if (result.Succeeded)
        {
            return null;
        }
        
        var errorString = string.Join("; ", result.Errors.Select(e => e.Description));
        return !string.IsNullOrEmpty(errorString) ? errorString : null;
    }
}
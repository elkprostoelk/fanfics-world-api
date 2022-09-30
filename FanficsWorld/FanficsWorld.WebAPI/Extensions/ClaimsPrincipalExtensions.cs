using System.Security.Claims;

namespace FanficsWorld.WebAPI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
}
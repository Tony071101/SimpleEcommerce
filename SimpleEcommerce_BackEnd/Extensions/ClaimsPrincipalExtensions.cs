using System.Security.Claims;

namespace SimpleEcommerce.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new UnauthorizedAccessException("User ID not found or invalid in token.");
            }
            return userId;
        }

        public static bool IsInRole(this ClaimsPrincipal user, string roleName)
        {
            return user.IsInRole(roleName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rommie.Api.Extensions
{
    public static class ClaimsExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("User ID claim is missing or invalid.");
            }
            return userId;
        }
        public static ICollection<string> GetRoles(this ClaimsPrincipal user)
        {
            return [.. user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)];
        }
    }
}
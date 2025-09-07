using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Rommie.Infrastructure.Authentication
{
    public class KeyCloackClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity!;

            var realmRoles = principal.FindFirst("realm_access")?.Value;
            if (realmRoles != null)
            {
                var roles = JsonDocument.Parse(realmRoles).RootElement
                    .GetProperty("roles")
                    .EnumerateArray()
                    .Select(r => r.GetString());

                foreach (var role in roles)
                    identity.AddClaim(new Claim(ClaimTypes.Role, role!));
            }

            return Task.FromResult(principal);
        }
    }
}
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Shared.Framework.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IIdentity"/>
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Add or replace claim.
        /// </summary>
        /// <param name="identity">Identity.</param>
        /// <param name="type">Claim type.</param>
        /// <param name="value">Claim value.</param>
        public static void AddOrReplaceClaim(this IIdentity identity, string type, string value)
        {
            var claimsIdentity = (ClaimsIdentity) identity;

            var claim = claimsIdentity.FindFirst(type);
            claimsIdentity.TryRemoveClaim(claim);
            
            if(value != null)
            {
                claimsIdentity.AddClaim(new Claim(type, value));
            }
        }

        /// <summary>
        /// Add or replace claim.
        /// </summary>
        /// <param name="identity">Identity.</param>
        /// <param name="claim">Claim value.</param>
        public static void AddOrReplaceClaim(this IIdentity identity, Claim claim)
        {
            var claimsIdentity = (ClaimsIdentity) identity;

            var oldClaim = claimsIdentity.FindFirst(claim.Type);
            claimsIdentity.TryRemoveClaim(oldClaim);
            claimsIdentity.AddClaim(claim);
        }

        /// <summary>
        /// Get claim value.
        /// </summary>
        /// <param name="identity">Identity.</param>
        /// <param name="type">Claim type.</param>
        /// <returns>Claim value as string.</returns>
        public static string GetClaimValue(this IIdentity identity, string type)
        {
            return ((ClaimsIdentity)identity).Claims
                .Where(claim => claim.Type == type)
                .Select(claim => claim.Value)
                .FirstOrDefault();
        }
    }
}
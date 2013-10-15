using System.Linq;
using System.Security.Principal;

namespace BookWorm.Helpers
{
    public static class PrincipalExtension
    {
        public static bool IsInRoles(this IPrincipal user, params string[] roles)
        {
            return roles.Any(user.IsInRole);
        }

    }
}

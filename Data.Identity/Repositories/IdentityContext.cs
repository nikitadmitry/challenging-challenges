using Data.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Identity.Repositories
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext() : base("IdentityConnection")
        {
        }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}
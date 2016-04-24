using System.Data.Entity;
using Data.Identity.Entities;

namespace Data.Identity.Context
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users
        {
            get;
            set;
        }

        public IdentityContext() : base("IdentityConnection")
        {
        }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}
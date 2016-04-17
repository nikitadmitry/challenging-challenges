using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public float Rating
        {
            get;
            set;
        }

        public int SolvedTasksQuantity
        {
            get;
            set;
        }

        public int PostedTasksQuantity
        {
            get;
            set;
        }

        private List<Achievement> achievements;

        public IList<Achievement> Achievements
        {
            get
            {
                return achievements ?? (achievements = new List<Achievement>());
            }
        }

        public string About
        {
            get;
            set;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Business.Identity.ViewModels
{
    public class IdentityUser : IUser<Guid>
    {
        public Guid Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string NormalizedUserName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string NormalizedEmail
        {
            get;
            set;
        }

        public string About
        {
            get;
            set;
        }

        public bool EmailConfirmed
        {
            get;
            set;
        }

        public double Rating
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

        public IList<string> Achievements;

        private IList<string> roles;

        public IList<string> Roles
        {
            get
            {
                return roles ?? (roles = new List<string>());
            }
            set
            {
                roles = value;
            }
        }

        public virtual string PasswordHash
        {
            get;
            set;
        }

        public virtual string SecurityStamp
        {
            get;
            set;
        }
    }
}
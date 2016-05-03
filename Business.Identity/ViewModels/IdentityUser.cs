using System;
using System.Collections.Generic;
using Data.Identity.Entities;
using Microsoft.AspNet.Identity;

namespace Business.Identity.ViewModels
{
    public class IdentityUser: IUser<Guid>
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

        public string Email
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
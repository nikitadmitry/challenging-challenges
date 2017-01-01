using System;
using Microsoft.AspNet.Identity;

namespace Business.Identity.ViewModels
{
    public class IdentityRole : IRole<Guid>
    {
        public Guid Id
        {
            get;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
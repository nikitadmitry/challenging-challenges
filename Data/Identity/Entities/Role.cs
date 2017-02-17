using System;
using Data.Common;
using Microsoft.AspNet.Identity;

namespace Data.Identity.Entities
{
    public class Role : Entity, IRole<Guid>
    {
        public string Name
        {
            get;
            set;
        }
    }
}
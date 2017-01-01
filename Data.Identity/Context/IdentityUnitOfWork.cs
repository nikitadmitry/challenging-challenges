using System;
using System.Data.Entity;
using Data.Common;
using Data.Identity.Entities;

namespace Data.Identity.Context
{
    public class IdentityUnitOfWork : UnitOfWork, IIdentityUnitOfWork
    {
        public IdentityUnitOfWork(
            Func<DbContext, IRepository<User>> usersRepositoryFunc,
            Func<DbContext, IRepository<Role>> rolesRepositoryFunc)
            : base(new IdentityContext())
        {
            RegisterRepository(usersRepositoryFunc(Context));
            RegisterRepository(rolesRepositoryFunc(Context));
        }

        public IRepository<User> UsersRepository
        {
            get
            {
                return GetRepository<User>();
            }
        }

        public IRepository<Role> RolesRepository
        {
            get
            {
                return GetRepository<Role>();
            }
        }
    }
}
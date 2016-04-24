using System;
using System.Data.Entity;
using Data.Common;
using Data.Identity.Entities;

namespace Data.Identity.Context
{
    public class IdentityUnitOfWork : UnitOfWork, IIdentityUnitOfWork
    {
        public IdentityUnitOfWork(Func<DbContext, IRepository<User>> usersRepositoryFunc)
            : base(new IdentityContext())
        {
            RegisterRepository(usersRepositoryFunc(Context));
        }

        public IRepository<User> UsersRepository
        {
            get
            {
                return GetRepository<User>();
            }
        }
    }
}
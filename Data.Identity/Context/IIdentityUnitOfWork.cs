using Data.Common;
using Data.Identity.Entities;

namespace Data.Identity.Context
{
    public interface IIdentityUnitOfWork: IUnitOfWork
    {
        IRepository<User> UsersRepository
        {
            get;
        }
    }
}
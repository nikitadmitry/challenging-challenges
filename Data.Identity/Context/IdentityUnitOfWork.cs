using System.Data.Entity;
using Data.Common;

namespace Data.Identity.Context
{
    public class IdentityUnitOfWork : UnitOfWork
    {
        public IdentityUnitOfWork(DbContext context)
            : base(context)
        {
        }
    }
}
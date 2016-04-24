using System;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Business.Identity.ViewModels;
using Data.Common.Query.Builder;
using Data.Common.Query.QueryParameters;
using Data.Identity.Context;
using Data.Identity.Entities;
using Shared.Framework.DataSource;
using Shared.Framework.Validation;

namespace Business.Identity
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityUnitOfWork unitOfWork;

        public IdentityService(IIdentityUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IdentityUser CreateIdentityUser(IdentityUser identityUser)
        {
            Contract.NotNull<ArgumentNullException>(identityUser);

            var user = Mapper.Map<User>(identityUser);

            var updatedUser = unitOfWork.InsertOrUpdate(user);

            unitOfWork.Commit();

            return Mapper.Map<IdentityUser>(updatedUser);
        }

        public IdentityUser UpdateIdentityUser(IdentityUser identityUser)
        {
            Contract.NotNull<ArgumentNullException>(identityUser);

            var user = unitOfWork.Get<User>(identityUser.Id);

            Mapper.Map(identityUser, user);

            var updatedUser = unitOfWork.InsertOrUpdate(user);

            unitOfWork.Commit();

            return Mapper.Map<IdentityUser>(updatedUser);
        }

        public IdentityUser GetIdentityUserByUserName(string userName)
        {
            var parameters = new BaseQueryParameters
            {
                FilterSettings = FilterSettingsBuilder<User>.Create()
                    .AddFilterRule(x => x.UserName, FilterOperator.IsEqualTo, userName)
                    .GetSettings()
            };

            var user = unitOfWork.GetSingleOrDefault<User>(parameters);

            return Mapper.Map<IdentityUser>(user);
        }

        public IdentityUser GetIdentityUserByEmail(string email)
        {
            var parameters = new BaseQueryParameters
            {
                FilterSettings = FilterSettingsBuilder<User>.Create()
                    .AddFilterRule(x => x.Email, FilterOperator.IsEqualTo, email)
                    .GetSettings()
            };

            var user = unitOfWork.GetSingleOrDefault<User>(parameters);

            return Mapper.Map<IdentityUser>(user);
        }

        public IdentityUser GetIdentityUserById(Guid userId)
        {
            var user = unitOfWork.Get<User>(userId);

            return Mapper.Map<IdentityUser>(user);
        }
    }
}

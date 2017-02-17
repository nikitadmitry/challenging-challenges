using System;
using System.Collections.Generic;
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
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)]
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public IdentityService(IIdentityUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IdentityUser CreateIdentityUser(IdentityUser identityUser)
        {
            Contract.NotNull<ArgumentNullException>(identityUser);

            var user = mapper.Map<User>(identityUser);

            var updatedUser = unitOfWork.InsertOrUpdate(user);

            unitOfWork.Commit();

            return mapper.Map<IdentityUser>(updatedUser);
        }

        public IdentityUser UpdateIdentityUser(IdentityUser identityUser)
        {
            Contract.NotNull<ArgumentNullException>(identityUser);

            var user = unitOfWork.Get<User>(identityUser.Id);

            mapper.Map(identityUser, user);

            var updatedUser = unitOfWork.InsertOrUpdate(user);

            unitOfWork.Commit();

            return mapper.Map<IdentityUser>(updatedUser);
        }

        public IdentityUser GetIdentityUserByUserName(string noramlizedUserName)
        {
            var parameters = new QueryParameters
            {
                FilterSettings = FilterSettingsBuilder<User>.Create()
                    .AddFilterRule(x => x.NormalizedUserName, FilterOperator.IsEqualTo, noramlizedUserName)
                    .GetSettings()
            };

            var user = unitOfWork.GetSingleOrDefault<User>(parameters);

            return mapper.Map<IdentityUser>(user);
        }

        public IdentityUser GetIdentityUserByEmail(string normalizedEmail)
        {
            var parameters = new QueryParameters
            {
                FilterSettings = FilterSettingsBuilder<User>.Create()
                    .AddFilterRule(x => x.NormalizedEmail, FilterOperator.IsEqualTo, normalizedEmail)
                    .GetSettings()
            };

            var user = unitOfWork.GetSingleOrDefault<User>(parameters);

            return mapper.Map<IdentityUser>(user);
        }

        public string GetUserNameById(Guid userId)
        {
            var user = unitOfWork.Get<User>(userId);

            return user.UserName;
        }

        public void AddRatingToUser(Guid userId, double rating)
        {
            var user = unitOfWork.Get<User>(userId);

            user.Rating += rating;

            unitOfWork.InsertOrUpdate(user);
            unitOfWork.Commit();
        }

        public IList<UserTopViewModel> GetTopUsers()
        {
            var parameters = new QueryParameters
            {
                SortSettings = SortSettingsBuilder<User>.Create()
                    .DescendingBy("Rating")
                    .GetSettings(),
                PageRule = new PageRule
                {
                    Count = 10,
                    Start = 0
                }
            };

            var users = unitOfWork.GetAll<User>(parameters);

            return mapper.Map<List<UserTopViewModel>>(users);
        }

        public void ConfirmEmail(Guid userId)
        {
            var user = unitOfWork.Get<User>(userId);

            user.EmailConfirmed = true;

            unitOfWork.InsertOrUpdate(user);
            unitOfWork.Commit();
        }

        public IdentityRole GetRoleById(Guid roleId)
        {
            var role = unitOfWork.Get<Role>(roleId);

            return mapper.Map<IdentityRole>(role);
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            var parameters = new QueryParameters
            {
                FilterSettings = FilterSettingsBuilder<Role>.Create()
                    .AddFilterRule(x => x.Name, FilterOperator.IsEqualTo, roleName)
                    .GetSettings()
            };

            var role = unitOfWork.GetSingleOrDefault<Role>(parameters);

            return mapper.Map<IdentityRole>(role);
        }

        public IdentityUser GetIdentityUserById(Guid userId)
        {
            var user = unitOfWork.Get<User>(userId);

            return mapper.Map<IdentityUser>(user);
        }
    }
}

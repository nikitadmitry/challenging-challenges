using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.Identity;
using Business.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Shared.Framework.Validation;

namespace Presentation.Web.Identity
{
    public class IdentityStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>, 
        IUserEmailStore<IdentityUser>, IUserRoleStore<IdentityUser>, IRoleStore<IdentityRole>
    {
        private readonly IIdentityService identityService;

        public IdentityStore(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.UserName = userName;

            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.NormalizedUserName = normalizedName;

            return Task.FromResult(0);
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var updatedUser = identityService.CreateIdentityUser(user);

            user.Id = updatedUser.Id;

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            identityService.UpdateIdentityUser(user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = identityService.GetIdentityUserById(userId.ToGuid());

            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = identityService.GetIdentityUserByUserName(normalizedUserName);

            return Task.FromResult(user);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);
            Contract.NotDefault<InvalidOperationException, Guid>(user.Id, "user id must not be default");

            if (confirmed)
            {
                identityService.ConfirmEmail(user.Id);
            }

            return Task.FromResult(0);
        }

        public Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            IdentityUser user = identityService.GetIdentityUserByEmail(normalizedEmail);
            
            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.Email);
        }

        public Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.NormalizedEmail = normalizedEmail;

            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            var roles = user.Roles ?? new List<string>();

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(role);

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(role);

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            Contract.NotNull<ArgumentNullException>(role);
            
            role.Name = roleName;

            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityRole> IRoleStore<IdentityRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityRole> IRoleStore<IdentityRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
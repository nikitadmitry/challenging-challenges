﻿using System;
using System.Threading.Tasks;
using Business.Identity;
using Business.Identity.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Shared.Framework.Validation;

namespace Challenging_Challenges.Identity
{
    public class UserStore : IUserPasswordStore<IdentityUser, Guid>, IUserEmailStore<IdentityUser, Guid>, IUserStore<IdentityUser, Guid>
    {
        private readonly IIdentityService identityService;

        public UserStore(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public Task CreateAsync(IdentityUser user)
        {
            identityService.CreateIdentityUser(user);

            return Task.FromResult(0);
        }

        public Task UpdateAsync(IdentityUser user)
        {
            identityService.UpdateIdentityUser(user);

            return Task.FromResult(0);
        }

        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            IdentityUser user = identityService.GetIdentityUserById(userId);

            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            IdentityUser user = identityService.GetIdentityUserByUserName(userName);

            return Task.FromResult(user);
        }

        public void Dispose()
        {
            //autofac handles this
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetEmailAsync(IdentityUser user, string email)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            Contract.NotNull<ArgumentNullException>(user);

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            Contract.NotNull<ArgumentNullException>(user);

            user.EmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            IdentityUser user = identityService.GetIdentityUserByEmail(email);

            return Task.FromResult(user);
        }
    }
}
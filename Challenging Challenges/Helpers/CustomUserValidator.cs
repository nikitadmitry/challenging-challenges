using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.Identity.ViewModels;
using Microsoft.AspNet.Identity;
using Shared.Framework.Resources;

namespace Challenging_Challenges.Helpers
{
    public class CustomUserValidator<TUser> : UserValidator<TUser, Guid> where TUser : IdentityUser
    {
        public CustomUserValidator(UserManager<TUser, Guid> manager) : base(manager)
        {
            Manager = manager;
        }

        private UserManager<TUser, Guid> Manager { get; }

        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var errors = new List<string>();
            await ValidateUserName(item, errors);
            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async Task ValidateUserName(TUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(String.Format(CultureInfo.CurrentCulture, Localization.PropertyTooShort, Localization.FullName));
            }
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, @"^[A-Za-z0-9@_\.]+$"))
            {
                errors.Add(String.Format(CultureInfo.CurrentCulture, Localization.InvalidUserName, user.UserName));
            }
            else
            {
                var owner = await Manager.FindByNameAsync(user.UserName);
                if (owner != null && !EqualityComparer<Guid>.Default.Equals(owner.Id, user.Id))
                {
                    errors.Add(String.Format(CultureInfo.CurrentCulture, Localization.DuplicateName, user.UserName));
                }
            }
        }

        private async Task ValidateEmail(TUser user, List<string> errors)
        {
            if (!user.Email.IsNullOrEmpty())
            {
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    errors.Add(String.Format(CultureInfo.CurrentCulture, Localization.PropertyTooShort, Localization.Email));
                    return;
                }
                try
                {
                    var m = new MailAddress(user.Email);
                }
                catch (FormatException)
                {
                    errors.Add(String.Format(CultureInfo.CurrentCulture, Localization.InvalidEmail, user.Email));
                    return;
                }
            }
            var owner = await Manager.FindByEmailAsync(user.Email);
            if (owner != null && !EqualityComparer<Guid>.Default.Equals(owner.Id, user.Id))
            {
                errors.Add(String.Format(CultureInfo.CurrentCulture, Localization.DuplicateEmail, user.Email));
            }
        }
    }
}

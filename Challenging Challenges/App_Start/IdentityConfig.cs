using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Challenging_Challenges.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IdentityUser = Business.Identity.ViewModels.IdentityUser;

namespace Challenging_Challenges
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridasync(message);
        }

        private async Task ConfigSendGridasync(IdentityMessage message)
        {
            var smtp = new SmtpClient(ConfigurationValuesProvider.Get<string>("SendGridMailAddress"), 587);

            var creds = new NetworkCredential(ConfigurationValuesProvider.Get<string>("SendGridUserName"),
                ConfigurationValuesProvider.Get<string>("SendGridPassword"));

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = creds;
            smtp.EnableSsl = false;

            var to = new MailAddress(message.Destination);
            var from = new MailAddress(ConfigurationValuesProvider.Get<string>("SendGridSentFromEmail"),
                ConfigurationValuesProvider.Get<string>("SendGridSentFromName"));

            var msg = new MailMessage();

            msg.To.Add(to);
            msg.From = from;
            msg.IsBodyHtml = true;
            msg.Subject = message.Subject;
            msg.Body = message.Body;

            await smtp.SendMailAsync(msg);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public sealed class ApplicationUserManager : UserManager<IdentityUser, Guid>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, Guid> store)
            : base(store)
        {
            UserValidator = new CustomUserValidator<IdentityUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false
            };

            UserTokenProvider = new DataProtectorTokenProvider<IdentityUser, Guid>(Startup.DataProtectionProvider.Create("ConfirmUser"));

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityUser, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();
            SmsService = new SmsService();
        }
    }
}
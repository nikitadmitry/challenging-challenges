using Data.Identity.Context;

namespace Challenging_Challenges.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
            ContextKey = "Challenging_Challenges.Models.Context.IdentityContext";
        }

        protected override void Seed(IdentityContext context)
        {
            //List<ApplicationUser> users = new List<ApplicationUser>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    var user = new ApplicationUser {
            //        Email = Faker.Internet.Email(),
            //        UserName = $"{Faker.Name.First()} {Faker.Name.Last()}",
            //        PasswordHash = Crypto.HashPassword("password"),
            //        SecurityStamp = Guid.NewGuid().ToString()
            //    };
            //    users.Add(user);
            //}

            //var uniqueUsers = users.DistinctBy(x => x.UserName).DistinctBy(x => x.Email);

            //foreach (var applicationUser in uniqueUsers)
            //{
            //    context.Users.AddOrUpdate(applicationUser);
            //}
        }
    }
}

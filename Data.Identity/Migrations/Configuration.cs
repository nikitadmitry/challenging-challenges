namespace Data.Identity.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Identity.Repositories.IdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.Identity.Repositories.IdentityContext context)
        {
        }
    }
}

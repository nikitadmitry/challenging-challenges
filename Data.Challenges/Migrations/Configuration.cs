using Data.Challenges.Context;

namespace Data.Challenges.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ChallengesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ChallengesContext context)
        {
        }
    }
}

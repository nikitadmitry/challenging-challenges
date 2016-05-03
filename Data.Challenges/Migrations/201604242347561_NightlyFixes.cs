namespace Data.Challenges.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NightlyFixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Challenges", "AuthorId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Challenges", "Rating", c => c.Double(nullable: false));
            AlterColumn("dbo.Solvers", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Solvers", "UserId", c => c.String());
            AlterColumn("dbo.Challenges", "Rating", c => c.Single(nullable: false));
            AlterColumn("dbo.Challenges", "AuthorId", c => c.String());
        }
    }
}

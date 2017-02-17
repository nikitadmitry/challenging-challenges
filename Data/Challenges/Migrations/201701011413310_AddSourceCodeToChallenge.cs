namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceCodeToChallenge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Challenges", "SolutionSourceCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Challenges", "SolutionSourceCode");
        }
    }
}

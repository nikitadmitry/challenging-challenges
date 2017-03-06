namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublicPropertyToTestCase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestCases", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestCases", "IsPublic");
        }
    }
}

namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NightkyFixes_ChangeDbSchema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Challenges", "TimeCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Challenges", "TimeCreated");
        }
    }
}

namespace Data.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNormalizedProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "NormalizedUserName", c => c.String());
            AddColumn("dbo.Users", "NormalizedEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "NormalizedEmail");
            DropColumn("dbo.Users", "NormalizedUserName");
        }
    }
}

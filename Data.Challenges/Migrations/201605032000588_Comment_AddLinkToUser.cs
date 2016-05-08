namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment_AddLinkToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "UserId", c => c.Guid(nullable: false));
            DropColumn("dbo.Comments", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "UserName", c => c.String());
            DropColumn("dbo.Comments", "UserId");
        }
    }
}

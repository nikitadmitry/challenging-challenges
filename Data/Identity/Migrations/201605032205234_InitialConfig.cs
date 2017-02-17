namespace Data.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Rating = c.Double(nullable: false),
                        SolvedTasksQuantity = c.Int(nullable: false),
                        PostedTasksQuantity = c.Int(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        About = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Achievements",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Value = c.String(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Achievements", "User_Id", "dbo.Users");
            DropIndex("dbo.Achievements", new[] { "User_Id" });
            DropTable("dbo.Achievements");
            DropTable("dbo.Users");
        }
    }
}

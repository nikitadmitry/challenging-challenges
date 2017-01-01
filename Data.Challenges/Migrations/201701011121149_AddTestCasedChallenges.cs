namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestCasedChallenges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestCases",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Challenge_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id)
                .Index(t => t.Challenge_Id);
            
            CreateTable(
                "dbo.CodeParameters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Value = c.String(),
                        Type = c.Int(nullable: false),
                        TestCase_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestCases", t => t.TestCase_Id)
                .Index(t => t.TestCase_Id);
            
            AddColumn("dbo.Challenges", "ChallengeType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestCases", "Challenge_Id", "dbo.Challenges");
            DropForeignKey("dbo.CodeParameters", "TestCase_Id", "dbo.TestCases");
            DropIndex("dbo.CodeParameters", new[] { "TestCase_Id" });
            DropIndex("dbo.TestCases", new[] { "Challenge_Id" });
            DropColumn("dbo.Challenges", "ChallengeType");
            DropTable("dbo.CodeParameters");
            DropTable("dbo.TestCases");
        }
    }
}

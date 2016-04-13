namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Challenges",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AuthorId = c.String(),
                        Title = c.String(),
                        PreviewText = c.String(),
                        Condition = c.String(),
                        Difficulty = c.Byte(nullable: false),
                        Section = c.Int(nullable: false),
                        Language = c.Int(nullable: false),
                        Rating = c.Single(nullable: false),
                        NumberOfVotes = c.Int(nullable: false),
                        TimesSolved = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Value = c.String(),
                        Challenge_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id, cascadeDelete: true)
                .Index(t => t.Challenge_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserName = c.String(),
                        Value = c.String(),
                        Challenge_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id, cascadeDelete: true)
                .Index(t => t.Challenge_Id);
            
            CreateTable(
                "dbo.Solvers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        HasSolved = c.Boolean(nullable: false),
                        HasRated = c.Boolean(nullable: false),
                        NumberOfTries = c.Byte(nullable: false),
                        Challenge_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id, cascadeDelete: true)
                .Index(t => t.Challenge_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagChallenges",
                c => new
                    {
                        Tag_Id = c.Guid(nullable: false),
                        Challenge_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Challenge_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Challenge_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagChallenges", "Challenge_Id", "dbo.Challenges");
            DropForeignKey("dbo.TagChallenges", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Solvers", "Challenge_Id", "dbo.Challenges");
            DropForeignKey("dbo.Comments", "Challenge_Id", "dbo.Challenges");
            DropForeignKey("dbo.Answers", "Challenge_Id", "dbo.Challenges");
            DropIndex("dbo.TagChallenges", new[] { "Challenge_Id" });
            DropIndex("dbo.TagChallenges", new[] { "Tag_Id" });
            DropIndex("dbo.Solvers", new[] { "Challenge_Id" });
            DropIndex("dbo.Comments", new[] { "Challenge_Id" });
            DropIndex("dbo.Answers", new[] { "Challenge_Id" });
            DropTable("dbo.TagChallenges");
            DropTable("dbo.Tags");
            DropTable("dbo.Solvers");
            DropTable("dbo.Comments");
            DropTable("dbo.Answers");
            DropTable("dbo.Challenges");
        }
    }
}

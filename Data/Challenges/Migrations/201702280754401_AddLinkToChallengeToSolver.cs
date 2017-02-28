namespace Data.Challenges.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkToChallengeToSolver : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Solvers", new[] { "Challenge_Id" });
            RenameColumn(table: "dbo.Solvers", name: "Challenge_Id", newName: "ChallengeId");
            AlterColumn("dbo.Solvers", "ChallengeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Solvers", "ChallengeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Solvers", new[] { "ChallengeId" });
            AlterColumn("dbo.Solvers", "ChallengeId", c => c.Guid());
            RenameColumn(table: "dbo.Solvers", name: "ChallengeId", newName: "Challenge_Id");
            CreateIndex("dbo.Solvers", "Challenge_Id");
        }
    }
}

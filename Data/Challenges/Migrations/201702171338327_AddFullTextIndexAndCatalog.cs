namespace Data.Challenges.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFullTextIndexAndCatalog : DbMigration
    {
        public override void Up()
        {
            SqlResource("Data.Challenges.Migrations.Sql.AddFullTextIndexAndCatalog.sql", suppressTransaction: true);
        }
    }
}

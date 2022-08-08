namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryTestResultModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntryTestResults", "programId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntryTestResults", "programId");
        }
    }
}

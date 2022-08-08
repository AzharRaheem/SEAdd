namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryTestResultModelPropUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntryTestResults", "year", c => c.String(nullable: false));
            AddColumn("dbo.EntryTestResults", "EntryTestStartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.EntryTestResults", "EntryTestEndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntryTestResults", "EntryTestEndTime");
            DropColumn("dbo.EntryTestResults", "EntryTestStartTime");
            DropColumn("dbo.EntryTestResults", "year");
        }
    }
}

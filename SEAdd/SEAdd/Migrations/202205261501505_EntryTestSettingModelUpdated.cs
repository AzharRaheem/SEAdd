namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryTestSettingModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntryTestSettings", "TotalQuestions", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntryTestSettings", "TotalQuestions");
        }
    }
}

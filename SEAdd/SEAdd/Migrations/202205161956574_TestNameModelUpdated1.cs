namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestNameModelUpdated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestNames", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestNames", "description");
        }
    }
}

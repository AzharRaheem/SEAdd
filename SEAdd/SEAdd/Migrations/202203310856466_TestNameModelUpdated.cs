namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestNameModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestNames", "departmentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestNames", "departmentName");
        }
    }
}

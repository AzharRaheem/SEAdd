namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentModelUpdated3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "EntryTestRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "EntryTestRequired");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentSettingModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepartmentSettings", "departmentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DepartmentSettings", "departmentName");
        }
    }
}

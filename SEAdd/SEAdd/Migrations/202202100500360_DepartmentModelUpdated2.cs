namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentModelUpdated2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "DeptLogUrl", c => c.String());
            DropColumn("dbo.Departments", "DeptLogorl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departments", "DeptLogorl", c => c.String());
            DropColumn("dbo.Departments", "DeptLogUrl");
        }
    }
}

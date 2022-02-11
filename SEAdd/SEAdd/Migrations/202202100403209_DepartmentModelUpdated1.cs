namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentModelUpdated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "BS", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departments", "MS", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departments", "PHD", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departments", "DeptLogorl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "DeptLogorl");
            DropColumn("dbo.Departments", "PHD");
            DropColumn("dbo.Departments", "MS");
            DropColumn("dbo.Departments", "BS");
        }
    }
}

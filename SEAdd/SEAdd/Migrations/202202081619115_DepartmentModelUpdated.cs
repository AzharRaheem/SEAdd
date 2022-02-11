namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "CampusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Departments", "CampusId");
            AddForeignKey("dbo.Departments", "CampusId", "dbo.Campus", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "CampusId", "dbo.Campus");
            DropIndex("dbo.Departments", new[] { "CampusId" });
            DropColumn("dbo.Departments", "CampusId");
        }
    }
}

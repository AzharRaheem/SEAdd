namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentPropAddedInMeritModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeritCriterias", "departmentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeritCriterias", "departmentName");
        }
    }
}

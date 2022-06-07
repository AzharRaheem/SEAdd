namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentSettingModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentSettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        isEntryTestRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DepartmentSettings");
        }
    }
}

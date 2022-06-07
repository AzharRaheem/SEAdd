namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryTestSettingModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntryTestSettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TestStartDateTime = c.DateTime(nullable: false),
                        TestEndDateTime = c.DateTime(nullable: false),
                        TotalMarks = c.Int(nullable: false),
                        testName = c.String(nullable: false),
                        departmentName = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EntryTestSettings");
        }
    }
}

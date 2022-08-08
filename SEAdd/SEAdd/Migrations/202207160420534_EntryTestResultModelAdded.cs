namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryTestResultModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntryTestResults",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.String(nullable: false),
                        rollNumber = c.String(nullable: false),
                        obtainedScorePercentage = c.Double(nullable: false),
                        totalScorePercentage = c.Double(nullable: false),
                        entryTestName = c.String(nullable: false),
                        departmentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EntryTestResults");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestNameModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestNames",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        testName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestNames");
        }
    }
}

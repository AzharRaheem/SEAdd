namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HostelAndTransportModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportRoutes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        route = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransportRoutes");
        }
    }
}

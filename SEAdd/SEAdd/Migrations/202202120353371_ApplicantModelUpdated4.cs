namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelUpdated4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hostels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CampusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Campus", t => t.CampusId, cascadeDelete: true)
                .Index(t => t.CampusId);
            
            AddColumn("dbo.Applicants", "isHostelRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applicants", "HostelName", c => c.String());
            AddColumn("dbo.Applicants", "isTransportRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applicants", "TransportRouteName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hostels", "CampusId", "dbo.Campus");
            DropIndex("dbo.Hostels", new[] { "CampusId" });
            DropColumn("dbo.Applicants", "TransportRouteName");
            DropColumn("dbo.Applicants", "isTransportRequired");
            DropColumn("dbo.Applicants", "HostelName");
            DropColumn("dbo.Applicants", "isHostelRequired");
            DropTable("dbo.Hostels");
        }
    }
}

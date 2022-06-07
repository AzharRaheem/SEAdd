namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeritCriteriaModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeritCriterias",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        BsMetricPercentage = c.Single(nullable: false),
                        BsIntermediatePercentage = c.Single(nullable: false),
                        BsEntryTestPercentage = c.Single(nullable: false),
                        MsMetricPercentage = c.Single(nullable: false),
                        MsIntermediatePercentage = c.Single(nullable: false),
                        MsBsPercentage = c.Single(nullable: false),
                        MsNTsORInterviewPercentage = c.Single(nullable: false),
                        PhdMetricPercentage = c.Single(nullable: false),
                        PhdIntermediatePercentage = c.Single(nullable: false),
                        PhdBsPercentage = c.Single(nullable: false),
                        PhdMsPercentage = c.Single(nullable: false),
                        PhdNTsORInterviewPercentage = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MeritCriterias");
        }
    }
}

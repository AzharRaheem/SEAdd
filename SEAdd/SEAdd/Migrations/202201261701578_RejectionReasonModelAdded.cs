namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionReasonModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RejectionReasons",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RejectionMessage = c.String(),
                        applicantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Applicants", t => t.applicantId, cascadeDelete: true)
                .Index(t => t.applicantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RejectionReasons", "applicantId", "dbo.Applicants");
            DropIndex("dbo.RejectionReasons", new[] { "applicantId" });
            DropTable("dbo.RejectionReasons");
        }
    }
}

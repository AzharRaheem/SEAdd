namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionMessageModelUpdated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RejectionMessages", "departmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.RejectionMessages", "applicantId");
            CreateIndex("dbo.RejectionMessages", "programId");
            CreateIndex("dbo.RejectionMessages", "departmentId");
            AddForeignKey("dbo.RejectionMessages", "applicantId", "dbo.Applicants", "id", cascadeDelete: true);
            AddForeignKey("dbo.RejectionMessages", "departmentId", "dbo.Departments", "id", cascadeDelete: true);
            AddForeignKey("dbo.RejectionMessages", "programId", "dbo.Programs", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RejectionMessages", "programId", "dbo.Programs");
            DropForeignKey("dbo.RejectionMessages", "departmentId", "dbo.Departments");
            DropForeignKey("dbo.RejectionMessages", "applicantId", "dbo.Applicants");
            DropIndex("dbo.RejectionMessages", new[] { "departmentId" });
            DropIndex("dbo.RejectionMessages", new[] { "programId" });
            DropIndex("dbo.RejectionMessages", new[] { "applicantId" });
            DropColumn("dbo.RejectionMessages", "departmentId");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionMessageModelUpdated2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RejectionMessages", "applicantId", "dbo.Applicants");
            DropForeignKey("dbo.RejectionMessages", "departmentId", "dbo.Departments");
            DropForeignKey("dbo.RejectionMessages", "programId", "dbo.Programs");
            DropIndex("dbo.RejectionMessages", new[] { "applicantId" });
            DropIndex("dbo.RejectionMessages", new[] { "programId" });
            DropIndex("dbo.RejectionMessages", new[] { "departmentId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.RejectionMessages", "departmentId");
            CreateIndex("dbo.RejectionMessages", "programId");
            CreateIndex("dbo.RejectionMessages", "applicantId");
            AddForeignKey("dbo.RejectionMessages", "programId", "dbo.Programs", "id", cascadeDelete: true);
            AddForeignKey("dbo.RejectionMessages", "departmentId", "dbo.Departments", "id", cascadeDelete: true);
            AddForeignKey("dbo.RejectionMessages", "applicantId", "dbo.Applicants", "id", cascadeDelete: true);
        }
    }
}

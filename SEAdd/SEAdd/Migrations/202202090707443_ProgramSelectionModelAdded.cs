namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramSelectionModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramSelections",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ApplicantId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Applicants", t => t.ApplicantId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ApplicantId)
                .Index(t => t.DepartmentId)
                .Index(t => t.ProgramId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramSelections", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramSelections", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.ProgramSelections", "ApplicantId", "dbo.Applicants");
            DropIndex("dbo.ProgramSelections", new[] { "ProgramId" });
            DropIndex("dbo.ProgramSelections", new[] { "DepartmentId" });
            DropIndex("dbo.ProgramSelections", new[] { "ApplicantId" });
            DropTable("dbo.ProgramSelections");
        }
    }
}

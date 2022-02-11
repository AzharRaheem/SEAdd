namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Applicants", "CampusId", "dbo.Campus");
            DropForeignKey("dbo.Applicants", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Applicants", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Applicants", "QotaId", "dbo.Qotas");
            DropForeignKey("dbo.RejectionReasons", "applicantId", "dbo.Applicants");
            DropIndex("dbo.Applicants", new[] { "CampusId" });
            DropIndex("dbo.Applicants", new[] { "DepartmentId" });
            DropIndex("dbo.Applicants", new[] { "ProgramId" });
            DropIndex("dbo.Applicants", new[] { "QotaId" });
            DropIndex("dbo.RejectionReasons", new[] { "applicantId" });
            DropTable("dbo.Applicants");
            DropTable("dbo.RejectionReasons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RejectionReasons",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RejectionMessage = c.String(),
                        applicantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FatherName = c.String(nullable: false, maxLength: 70),
                        Email = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        CNIC = c.String(nullable: false),
                        PersonalContact = c.String(nullable: false),
                        GuardianContact = c.String(nullable: false),
                        PermanentAddress = c.String(nullable: false),
                        PostalAddress = c.String(nullable: false),
                        StateProvince = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Domicile = c.String(nullable: false),
                        profileImgUrl = c.String(),
                        MetricTotalMarks = c.Double(nullable: false),
                        MetricObtainedMarks = c.Double(nullable: false),
                        MetricProgram = c.String(nullable: false),
                        MetricBoard = c.String(nullable: false),
                        MetricGrade = c.String(nullable: false),
                        MetricDivision = c.String(nullable: false),
                        MetricPercentage = c.Double(nullable: false),
                        MetricYearOfPassing = c.String(nullable: false),
                        MetricRollNumber = c.String(nullable: false),
                        MetricMarksSheetUrl = c.String(),
                        FScTotalMarks = c.Double(nullable: false),
                        FScObtainedMarks = c.Double(nullable: false),
                        FScProgram = c.String(nullable: false),
                        FScBoard = c.String(nullable: false),
                        FScGrade = c.String(nullable: false),
                        FScDivision = c.String(nullable: false),
                        FScPercentage = c.Double(nullable: false),
                        FScYearOfPassing = c.String(nullable: false),
                        FScRollNumber = c.String(nullable: false),
                        FScMarksSheetUrl = c.String(),
                        CampusId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        QotaId = c.Int(nullable: false),
                        isApproved = c.Boolean(nullable: false),
                        applyDate = c.DateTime(nullable: false),
                        isRejected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.RejectionReasons", "applicantId");
            CreateIndex("dbo.Applicants", "QotaId");
            CreateIndex("dbo.Applicants", "ProgramId");
            CreateIndex("dbo.Applicants", "DepartmentId");
            CreateIndex("dbo.Applicants", "CampusId");
            AddForeignKey("dbo.RejectionReasons", "applicantId", "dbo.Applicants", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Applicants", "QotaId", "dbo.Qotas", "id", cascadeDelete: true);
            AddForeignKey("dbo.Applicants", "ProgramId", "dbo.Programs", "id", cascadeDelete: true);
            AddForeignKey("dbo.Applicants", "DepartmentId", "dbo.Departments", "id", cascadeDelete: true);
            AddForeignKey("dbo.Applicants", "CampusId", "dbo.Campus", "id", cascadeDelete: true);
        }
    }
}

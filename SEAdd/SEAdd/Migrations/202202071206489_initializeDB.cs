namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initializeDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdmissionDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 500),
                        NotificationDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        NotificationFileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campus", t => t.CampusId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.Qotas", t => t.QotaId, cascadeDelete: true)
                .Index(t => t.CampusId)
                .Index(t => t.DepartmentId)
                .Index(t => t.ProgramId)
                .Index(t => t.QotaId);
            
            CreateTable(
                "dbo.Campus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                        Location = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ProgramName = c.String(nullable: false, maxLength: 50),
                        duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Qotas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        numberOfSeats = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
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
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        accountNo = c.String(nullable: false, maxLength: 14),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Fees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        formFee = c.Double(nullable: false),
                        entryTestFee = c.Double(nullable: false),
                        bankId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Banks", t => t.bankId, cascadeDelete: true)
                .Index(t => t.bankId);
            
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationDate = c.DateTime(nullable: false),
                        NotificationTitle = c.String(nullable: false, maxLength: 100),
                        NotificationFileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Cnic = c.String(nullable: false, maxLength: 15),
                        profileImgUrl = c.String(),
                        fatherName = c.String(),
                        address = c.String(),
                        gender = c.Boolean(),
                        department = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Fees", "bankId", "dbo.Banks");
            DropForeignKey("dbo.RejectionReasons", "applicantId", "dbo.Applicants");
            DropForeignKey("dbo.Applicants", "QotaId", "dbo.Qotas");
            DropForeignKey("dbo.Applicants", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Applicants", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Applicants", "CampusId", "dbo.Campus");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Fees", new[] { "bankId" });
            DropIndex("dbo.RejectionReasons", new[] { "applicantId" });
            DropIndex("dbo.Applicants", new[] { "QotaId" });
            DropIndex("dbo.Applicants", new[] { "ProgramId" });
            DropIndex("dbo.Applicants", new[] { "DepartmentId" });
            DropIndex("dbo.Applicants", new[] { "CampusId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.Boards");
            DropTable("dbo.Fees");
            DropTable("dbo.Banks");
            DropTable("dbo.RejectionReasons");
            DropTable("dbo.Qotas");
            DropTable("dbo.Programs");
            DropTable("dbo.Departments");
            DropTable("dbo.Campus");
            DropTable("dbo.Applicants");
            DropTable("dbo.AdmissionDates");
        }
    }
}

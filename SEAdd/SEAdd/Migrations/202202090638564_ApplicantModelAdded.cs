namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        FullName = c.String(nullable: false, maxLength: 50),
                        FatherName = c.String(nullable: false, maxLength: 70),
                        Email = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        CNIC = c.String(nullable: false),
                        PersonalContact = c.String(nullable: false),
                        GuardianContact = c.String(nullable: false),
                        Nationality = c.String(nullable: false),
                        Religion = c.String(nullable: false),
                        Domicile = c.String(nullable: false),
                        QotaId = c.Int(nullable: false),
                        PermanentAddress = c.String(nullable: false),
                        PostalAddress = c.String(nullable: false),
                        ProvienceId = c.Int(nullable: false),
                        profileImgUrl = c.String(),
                        District = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Proviences", t => t.ProvienceId, cascadeDelete: true)
                .ForeignKey("dbo.Qotas", t => t.QotaId, cascadeDelete: true)
                .Index(t => t.QotaId)
                .Index(t => t.ProvienceId);
            
            CreateTable(
                "dbo.Proviences",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Academics", "ApplicantId", c => c.Int(nullable: false));
            CreateIndex("dbo.Academics", "ApplicantId");
            AddForeignKey("dbo.Academics", "ApplicantId", "dbo.Applicants", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Academics", "ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.Applicants", "QotaId", "dbo.Qotas");
            DropForeignKey("dbo.Applicants", "ProvienceId", "dbo.Proviences");
            DropIndex("dbo.Applicants", new[] { "ProvienceId" });
            DropIndex("dbo.Applicants", new[] { "QotaId" });
            DropIndex("dbo.Academics", new[] { "ApplicantId" });
            DropColumn("dbo.Academics", "ApplicantId");
            DropTable("dbo.Proviences");
            DropTable("dbo.Applicants");
        }
    }
}

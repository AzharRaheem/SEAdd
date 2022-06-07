namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantQualificationProgramsModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicantQualificationPrograms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        FullName = c.String(),
                        FatherName = c.String(),
                        Email = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.String(),
                        CNIC = c.String(),
                        PersonalContact = c.String(),
                        GuardianContact = c.String(),
                        Nationality = c.String(),
                        Religion = c.String(),
                        Domicile = c.String(),
                        Quota = c.String(),
                        PermanentAddress = c.String(),
                        PostalAddress = c.String(),
                        Provience = c.String(),
                        profileImgUrl = c.String(),
                        District = c.String(),
                        ApplyDate = c.DateTime(nullable: false),
                        M_AcademicDegree = c.String(),
                        M_Discipline = c.String(),
                        M_YearOfPassing = c.String(),
                        M_RollNumber = c.String(),
                        M_boardOrInstitute = c.String(),
                        M_CountryName = c.String(),
                        M_TotalMarks = c.Double(nullable: false),
                        M_ObtainedMarks = c.Double(nullable: false),
                        M_Percentage = c.Double(nullable: false),
                        I_AcademicDegree = c.String(),
                        I_Discipline = c.String(),
                        I_YearOfPassing = c.String(),
                        I_RollNumber = c.String(),
                        I_boardOrInstitute = c.String(),
                        I_CountryName = c.String(),
                        I_TotalMarks = c.Double(nullable: false),
                        I_ObtainedMarks = c.Double(nullable: false),
                        I_Percentage = c.Double(nullable: false),
                        DepartmentName = c.String(),
                        ProgramName = c.String(),
                        ProgramType = c.String(),
                        duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplicantQualificationPrograms");
        }
    }
}

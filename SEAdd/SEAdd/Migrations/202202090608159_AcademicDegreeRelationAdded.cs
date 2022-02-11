namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcademicDegreeRelationAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Academics",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AcademicDegree = c.String(nullable: false),
                        Discipline = c.String(nullable: false),
                        YearOfPassing = c.String(nullable: false),
                        RollNumber = c.String(nullable: false),
                        boardOrInstitute = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        TotalMarks = c.Double(nullable: false),
                        ObtainedMarks = c.Double(nullable: false),
                        Percentage = c.Double(nullable: false),
                        DMcMarksSheetUrl = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.AcademicDegrees", "Academic_id", c => c.Int());
            CreateIndex("dbo.AcademicDegrees", "Academic_id");
            AddForeignKey("dbo.AcademicDegrees", "Academic_id", "dbo.Academics", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AcademicDegrees", "Academic_id", "dbo.Academics");
            DropForeignKey("dbo.Academics", "CountryId", "dbo.Countries");
            DropIndex("dbo.Academics", new[] { "CountryId" });
            DropIndex("dbo.AcademicDegrees", new[] { "Academic_id" });
            DropColumn("dbo.AcademicDegrees", "Academic_id");
            DropTable("dbo.Countries");
            DropTable("dbo.Academics");
        }
    }
}

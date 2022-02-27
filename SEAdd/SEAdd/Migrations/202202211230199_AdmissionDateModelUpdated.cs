namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdmissionDateModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdmissionDates", "UnderGraduateProgStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdmissionDates", "UnderGraduateProgEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdmissionDates", "GraduateProgStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdmissionDates", "GraduateProgEndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdmissionDates", "StartDate");
            DropColumn("dbo.AdmissionDates", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdmissionDates", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdmissionDates", "StartDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdmissionDates", "GraduateProgEndDate");
            DropColumn("dbo.AdmissionDates", "GraduateProgStartDate");
            DropColumn("dbo.AdmissionDates", "UnderGraduateProgEndDate");
            DropColumn("dbo.AdmissionDates", "UnderGraduateProgStartDate");
        }
    }
}

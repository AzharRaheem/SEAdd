namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantQualificationProgramsModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicantQualificationPrograms", "campus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicantQualificationPrograms", "campus");
        }
    }
}

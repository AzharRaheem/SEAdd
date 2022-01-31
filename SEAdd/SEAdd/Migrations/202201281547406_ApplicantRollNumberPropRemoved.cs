namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantRollNumberPropRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Applicants", "ApplicantRollNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Applicants", "ApplicantRollNumber", c => c.String());
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "ApplyDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "ApplyDate");
        }
    }
}

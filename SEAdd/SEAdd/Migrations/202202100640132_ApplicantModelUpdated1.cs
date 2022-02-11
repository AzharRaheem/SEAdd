namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelUpdated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "isRegistrationFinished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "isRegistrationFinished");
        }
    }
}

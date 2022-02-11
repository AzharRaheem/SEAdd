namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelUpdated3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "isRejected", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applicants", "isApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "isApproved");
            DropColumn("dbo.Applicants", "isRejected");
        }
    }
}

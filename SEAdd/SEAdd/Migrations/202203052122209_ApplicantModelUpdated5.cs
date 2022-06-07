namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantModelUpdated5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Applicants", "isRejected");
            DropColumn("dbo.Applicants", "isApproved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Applicants", "isApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applicants", "isRejected", c => c.Boolean(nullable: false));
        }
    }
}

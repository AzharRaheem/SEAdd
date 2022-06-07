namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isApprovedAndRejectedPropAdded_Applicant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "isApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applicants", "isRejected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "isRejected");
            DropColumn("dbo.Applicants", "isApproved");
        }
    }
}

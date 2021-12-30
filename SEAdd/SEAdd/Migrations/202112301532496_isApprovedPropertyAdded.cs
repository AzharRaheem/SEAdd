namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isApprovedPropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "isApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "isApproved");
        }
    }
}

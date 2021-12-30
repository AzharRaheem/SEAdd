namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MobileNumberValidationAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applicants", "GuardianContact", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applicants", "GuardianContact", c => c.String());
        }
    }
}

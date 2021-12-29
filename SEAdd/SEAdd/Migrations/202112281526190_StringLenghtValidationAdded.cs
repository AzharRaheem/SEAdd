namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringLenghtValidationAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applicants", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Applicants", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Applicants", "FatherName", c => c.String(nullable: false, maxLength: 70));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applicants", "FatherName", c => c.String(nullable: false));
            AlterColumn("dbo.Applicants", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Applicants", "FirstName", c => c.String(nullable: false));
        }
    }
}

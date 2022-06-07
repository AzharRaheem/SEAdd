namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantDeptProgramPropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "departmentName", c => c.String());
            AddColumn("dbo.Applicants", "programName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "programName");
            DropColumn("dbo.Applicants", "departmentName");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeritListModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeritLists", "email", c => c.String(nullable: false));
            AddColumn("dbo.MeritLists", "metricObtainedMarks", c => c.Double(nullable: false));
            AddColumn("dbo.MeritLists", "intermediateObtainedMarks", c => c.Double(nullable: false));
            AddColumn("dbo.MeritLists", "metricTotalMarks", c => c.Double(nullable: false));
            AddColumn("dbo.MeritLists", "intermediateTotalMarks", c => c.Double(nullable: false));
            AddColumn("dbo.MeritLists", "annualPassingYear", c => c.String(nullable: false));
            AddColumn("dbo.MeritLists", "meritListYear", c => c.String(nullable: false));
            AlterColumn("dbo.MeritLists", "programName", c => c.String(nullable: false));
            AlterColumn("dbo.MeritLists", "departmentName", c => c.String(nullable: false));
            AlterColumn("dbo.MeritLists", "fullName", c => c.String(nullable: false));
            AlterColumn("dbo.MeritLists", "fatherName", c => c.String(nullable: false));
            AlterColumn("dbo.MeritLists", "cnic", c => c.String(nullable: false));
            AlterColumn("dbo.MeritLists", "QuotaName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MeritLists", "QuotaName", c => c.String());
            AlterColumn("dbo.MeritLists", "cnic", c => c.String());
            AlterColumn("dbo.MeritLists", "fatherName", c => c.String());
            AlterColumn("dbo.MeritLists", "fullName", c => c.String());
            AlterColumn("dbo.MeritLists", "departmentName", c => c.String());
            AlterColumn("dbo.MeritLists", "programName", c => c.String());
            DropColumn("dbo.MeritLists", "meritListYear");
            DropColumn("dbo.MeritLists", "annualPassingYear");
            DropColumn("dbo.MeritLists", "intermediateTotalMarks");
            DropColumn("dbo.MeritLists", "metricTotalMarks");
            DropColumn("dbo.MeritLists", "intermediateObtainedMarks");
            DropColumn("dbo.MeritLists", "metricObtainedMarks");
            DropColumn("dbo.MeritLists", "email");
        }
    }
}

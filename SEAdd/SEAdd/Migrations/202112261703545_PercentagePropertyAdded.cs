namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PercentagePropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "MetricPercentage", c => c.Double(nullable: false));
            AddColumn("dbo.Applicants", "FScPercentage", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "FScPercentage");
            DropColumn("dbo.Applicants", "MetricPercentage");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applyDatePropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "applyDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "applyDate");
        }
    }
}

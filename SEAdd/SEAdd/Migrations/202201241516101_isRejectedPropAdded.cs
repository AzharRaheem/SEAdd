namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isRejectedPropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "isRejected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applicants", "isRejected");
        }
    }
}

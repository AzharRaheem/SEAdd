namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionMessageDatePropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RejectionMessages", "date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RejectionMessages", "date");
        }
    }
}

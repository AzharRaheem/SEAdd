namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampusModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campus", "Location", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campus", "Location");
        }
    }
}

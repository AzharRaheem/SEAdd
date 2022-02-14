namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeeModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fees", "additionalFormFee", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fees", "additionalFormFee");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionMessageModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RejectionMessages", "programId", c => c.Int(nullable: false));
            AlterColumn("dbo.RejectionMessages", "message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RejectionMessages", "message", c => c.String());
            DropColumn("dbo.RejectionMessages", "programId");
        }
    }
}

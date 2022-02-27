namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramSelectionModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgramSelections", "isRejected", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProgramSelections", "isApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgramSelections", "isApproved");
            DropColumn("dbo.ProgramSelections", "isRejected");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "duration");
        }
    }
}

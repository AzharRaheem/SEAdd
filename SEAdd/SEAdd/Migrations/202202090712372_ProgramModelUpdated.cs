namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "ProgramType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "ProgramType");
        }
    }
}

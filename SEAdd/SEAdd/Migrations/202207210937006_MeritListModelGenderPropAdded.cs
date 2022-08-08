namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeritListModelGenderPropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeritLists", "gender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeritLists", "gender");
        }
    }
}

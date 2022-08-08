namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isVisiblePropAddedInMeritListModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeritLists", "isVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeritLists", "isVisible");
        }
    }
}

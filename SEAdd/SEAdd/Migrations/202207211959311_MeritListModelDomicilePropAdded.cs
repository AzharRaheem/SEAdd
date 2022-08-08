namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeritListModelDomicilePropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeritLists", "domicile", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeritLists", "domicile");
        }
    }
}

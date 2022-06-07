namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuotaModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Qotas", "NominationFrom", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Qotas", "NominationFrom");
        }
    }
}

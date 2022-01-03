namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationDatePropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdmissionDates", "NotificationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdmissionDates", "NotificationDate");
        }
    }
}

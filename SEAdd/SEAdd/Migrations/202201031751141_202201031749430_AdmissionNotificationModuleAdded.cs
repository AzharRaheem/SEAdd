namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202201031749430_AdmissionNotificationModuleAdded : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AdmissionDates", "NotificationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdmissionDates", "NotificationDate", c => c.DateTime(nullable: false));
        }
    }
}

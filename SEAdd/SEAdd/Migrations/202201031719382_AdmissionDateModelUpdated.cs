namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdmissionDateModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdmissionDates", "NotificationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdmissionDates", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdmissionDates", "Description", c => c.String(nullable: false, maxLength: 1000));
            DropColumn("dbo.AdmissionDates", "NotificationDate");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationDate = c.DateTime(nullable: false),
                        NotificationTitle = c.String(nullable: false, maxLength: 100),
                        NotificationFileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
        }
    }
}

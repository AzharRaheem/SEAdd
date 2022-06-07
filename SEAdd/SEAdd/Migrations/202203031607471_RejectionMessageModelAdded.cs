namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionMessageModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RejectionMessages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        applicantId = c.Int(nullable: false),
                        title = c.String(),
                        message = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RejectionMessages");
        }
    }
}

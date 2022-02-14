namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantTransportPropAdded1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applicants", "isTransportRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applicants", "isTransportRequired", c => c.Boolean());
        }
    }
}

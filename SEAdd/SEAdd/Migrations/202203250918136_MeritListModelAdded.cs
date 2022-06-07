namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeritListModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeritLists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        formNo = c.Int(nullable: false),
                        rollNo = c.String(),
                        programName = c.String(),
                        departmentName = c.String(),
                        fullName = c.String(),
                        fatherName = c.String(),
                        cnic = c.String(),
                        scores = c.Single(nullable: false),
                        nominationFrom = c.String(),
                        QuotaName = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MeritLists");
        }
    }
}

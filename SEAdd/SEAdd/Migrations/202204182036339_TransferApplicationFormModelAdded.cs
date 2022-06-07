namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransferApplicationFormModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransferApplicationForms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        programId = c.Int(nullable: false),
                        oldDepartmentId = c.Int(nullable: false),
                        newDepartmentId = c.Int(nullable: false),
                        applicantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransferApplicationForms");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicantRollNumberModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RollNumbers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        applicantId = c.Int(nullable: false),
                        userId = c.String(),
                        applicantRollNumber = c.String(),
                        programId = c.Int(nullable: false),
                        departmentName = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RollNumbers");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcademicDegreeModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicDegrees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AcademicDegrees");
        }
    }
}

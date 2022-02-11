namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcademicDegreeModelUpdated1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AcademicDegrees", "Academic_id", "dbo.Academics");
            DropIndex("dbo.AcademicDegrees", new[] { "Academic_id" });
            DropColumn("dbo.AcademicDegrees", "Academic_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AcademicDegrees", "Academic_id", c => c.Int());
            CreateIndex("dbo.AcademicDegrees", "Academic_id");
            AddForeignKey("dbo.AcademicDegrees", "Academic_id", "dbo.Academics", "id");
        }
    }
}

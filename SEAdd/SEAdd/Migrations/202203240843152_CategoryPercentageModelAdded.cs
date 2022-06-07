namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryPercentageModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryPercentages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        categoryId = c.Int(nullable: false),
                        department = c.String(),
                        QuestionPercentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.TestCategories", t => t.categoryId, cascadeDelete: true)
                .Index(t => t.categoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryPercentages", "categoryId", "dbo.TestCategories");
            DropIndex("dbo.CategoryPercentages", new[] { "categoryId" });
            DropTable("dbo.CategoryPercentages");
        }
    }
}

namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryPercentageModelUpdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryPercentages", "categoryId", "dbo.TestCategories");
            DropIndex("dbo.CategoryPercentages", new[] { "categoryId" });
            RenameColumn(table: "dbo.CategoryPercentages", name: "categoryId", newName: "TestCategory_id");
            AddColumn("dbo.CategoryPercentages", "categoryName", c => c.String(nullable: false));
            AlterColumn("dbo.CategoryPercentages", "TestCategory_id", c => c.Int());
            CreateIndex("dbo.CategoryPercentages", "TestCategory_id");
            AddForeignKey("dbo.CategoryPercentages", "TestCategory_id", "dbo.TestCategories", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryPercentages", "TestCategory_id", "dbo.TestCategories");
            DropIndex("dbo.CategoryPercentages", new[] { "TestCategory_id" });
            AlterColumn("dbo.CategoryPercentages", "TestCategory_id", c => c.Int(nullable: false));
            DropColumn("dbo.CategoryPercentages", "categoryName");
            RenameColumn(table: "dbo.CategoryPercentages", name: "TestCategory_id", newName: "categoryId");
            CreateIndex("dbo.CategoryPercentages", "categoryId");
            AddForeignKey("dbo.CategoryPercentages", "categoryId", "dbo.TestCategories", "id", cascadeDelete: true);
        }
    }
}

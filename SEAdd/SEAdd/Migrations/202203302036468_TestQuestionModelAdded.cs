namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestQuestionModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestQuestions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false),
                        OptionA = c.String(nullable: false),
                        OptionB = c.String(nullable: false),
                        OptionC = c.String(nullable: false),
                        OptionD = c.String(nullable: false),
                        CorrectAns = c.String(nullable: false),
                        Hint = c.String(nullable: false),
                        marks = c.Int(nullable: false),
                        departmentName = c.String(),
                        categoryId = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                        testTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CategoryPercentages", t => t.categoryId, cascadeDelete: true)
                .ForeignKey("dbo.TestNames", t => t.TestId, cascadeDelete: true)
                .ForeignKey("dbo.TestTypes", t => t.testTypeId, cascadeDelete: true)
                .Index(t => t.categoryId)
                .Index(t => t.TestId)
                .Index(t => t.testTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestQuestions", "testTypeId", "dbo.TestTypes");
            DropForeignKey("dbo.TestQuestions", "TestId", "dbo.TestNames");
            DropForeignKey("dbo.TestQuestions", "categoryId", "dbo.CategoryPercentages");
            DropIndex("dbo.TestQuestions", new[] { "testTypeId" });
            DropIndex("dbo.TestQuestions", new[] { "TestId" });
            DropIndex("dbo.TestQuestions", new[] { "categoryId" });
            DropTable("dbo.TestQuestions");
        }
    }
}

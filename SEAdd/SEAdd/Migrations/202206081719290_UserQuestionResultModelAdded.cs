namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserQuestionResultModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserQuestionResults",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        applicantId = c.Int(nullable: false),
                        rollNumber = c.String(),
                        departmentName = c.String(),
                        QuestionId = c.Int(nullable: false),
                        userAns = c.String(),
                        correctAns = c.String(),
                        TotalMarks = c.Int(nullable: false),
                        ObtainedMarks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Applicants", t => t.applicantId, cascadeDelete: true)
                .ForeignKey("dbo.TestQuestions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.applicantId)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserQuestionResults", "QuestionId", "dbo.TestQuestions");
            DropForeignKey("dbo.UserQuestionResults", "applicantId", "dbo.Applicants");
            DropIndex("dbo.UserQuestionResults", new[] { "QuestionId" });
            DropIndex("dbo.UserQuestionResults", new[] { "applicantId" });
            DropTable("dbo.UserQuestionResults");
        }
    }
}

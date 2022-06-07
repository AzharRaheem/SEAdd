namespace SEAdd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestQuestionModelUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TestQuestions", "Hint", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TestQuestions", "Hint", c => c.String(nullable: false));
        }
    }
}

namespace Project_Kanban.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailInActor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_M_Actor", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TB_M_Actor", "Email");
        }
    }
}

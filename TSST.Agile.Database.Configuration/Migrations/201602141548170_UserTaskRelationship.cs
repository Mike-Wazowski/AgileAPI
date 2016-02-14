namespace TSST.Agile.Database.Configuration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTaskRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "UserId");
            AddForeignKey("dbo.Tasks", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "UserId", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "UserId" });
            DropColumn("dbo.Tasks", "UserId");
        }
    }
}

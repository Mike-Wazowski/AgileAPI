namespace TSST.Agile.Database.Configuration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendsList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FriendId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FriendId);
            
            AddColumn("dbo.Tasks", "CreationDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "CompleteDate", c => c.DateTime());
            DropColumn("dbo.Tasks", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "EndDate", c => c.DateTime());
            DropForeignKey("dbo.Friendships", "UserId", "dbo.Users");
            DropForeignKey("dbo.Friendships", "FriendId", "dbo.Users");
            DropIndex("dbo.Friendships", new[] { "FriendId" });
            DropIndex("dbo.Friendships", new[] { "UserId" });
            DropColumn("dbo.Tasks", "CompleteDate");
            DropColumn("dbo.Tasks", "CreationDate");
            DropTable("dbo.Friendships");
        }
    }
}

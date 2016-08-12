namespace ChieftensLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notnull_userId_foreignKey_on_TurnIns_and_SharedFile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SharedFiles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TurnIns", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SharedFiles", new[] { "UserId" });
            DropIndex("dbo.TurnIns", new[] { "UserId" });
            AlterColumn("dbo.SharedFiles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TurnIns", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.SharedFiles", "UserId");
            CreateIndex("dbo.TurnIns", "UserId");
            AddForeignKey("dbo.SharedFiles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TurnIns", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TurnIns", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SharedFiles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TurnIns", new[] { "UserId" });
            DropIndex("dbo.SharedFiles", new[] { "UserId" });
            AlterColumn("dbo.TurnIns", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.SharedFiles", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TurnIns", "UserId");
            CreateIndex("dbo.SharedFiles", "UserId");
            AddForeignKey("dbo.TurnIns", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SharedFiles", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}

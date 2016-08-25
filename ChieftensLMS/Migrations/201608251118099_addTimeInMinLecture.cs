namespace ChieftensLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTimeInMinLecture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "TimeInMin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lectures", "TimeInMin");
        }
    }
}

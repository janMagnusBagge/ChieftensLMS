namespace ChieftensLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class afsasdf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        ExpirationDate = c.DateTime(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.SharedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        FileName = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SurName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TurnIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        FileName = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.CourseUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CourseId })
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TurnIns", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.TurnIns", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.SharedFiles", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.CourseUsers", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseUsers", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.SharedFiles", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Lectures", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Assignments", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseUsers", new[] { "CourseId" });
            DropIndex("dbo.CourseUsers", new[] { "UserId" });
            DropIndex("dbo.TurnIns", new[] { "AssignmentId" });
            DropIndex("dbo.TurnIns", new[] { "UserId" });
            DropIndex("dbo.SharedFiles", new[] { "CourseId" });
            DropIndex("dbo.SharedFiles", new[] { "UserId" });
            DropIndex("dbo.Lectures", new[] { "CourseId" });
            DropIndex("dbo.Assignments", new[] { "CourseId" });
            DropTable("dbo.CourseUsers");
            DropTable("dbo.TurnIns");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.SharedFiles");
            DropTable("dbo.Lectures");
            DropTable("dbo.Courses");
            DropTable("dbo.Assignments");
        }
    }
}

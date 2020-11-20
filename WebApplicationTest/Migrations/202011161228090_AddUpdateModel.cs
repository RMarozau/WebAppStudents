namespace WebApplicationTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        CountPeople = c.Int(nullable: false),
                        Reiting = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClassId);
            
            CreateTable(
                "dbo.Shedule",
                c => new
                    {
                        SheduleId = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(),
                        ClassId = c.Int(),
                        NumberObject = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SheduleId)
                .ForeignKey("dbo.Class", t => t.ClassId)
                .ForeignKey("dbo.Teacher", t => t.TeacherId)
                .Index(t => t.TeacherId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Teacher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIOteacher = c.String(nullable: false, maxLength: 100),
                        Birthday = c.DateTime(nullable: false),
                        Exp = c.Int(nullable: false),
                        CountHour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIO = c.String(nullable: false, maxLength: 100),
                        Birtday = c.DateTime(nullable: false),
                        ClassId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.ClassId)
                .Index(t => t.ClassId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Student", "ClassId", "dbo.Class");
            DropForeignKey("dbo.Shedule", "TeacherId", "dbo.Teacher");
            DropForeignKey("dbo.Shedule", "ClassId", "dbo.Class");
            DropIndex("dbo.Student", new[] { "ClassId" });
            DropIndex("dbo.Shedule", new[] { "ClassId" });
            DropIndex("dbo.Shedule", new[] { "TeacherId" });
            DropTable("dbo.Student");
            DropTable("dbo.Teacher");
            DropTable("dbo.Shedule");
            DropTable("dbo.Class");
        }
    }
}

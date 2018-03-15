namespace FYstudentMgr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addenrollservice1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EnrollService",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnrollmentId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        PostUserId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enrollment", t => t.EnrollmentId)
                .ForeignKey("dbo.PostUser", t => t.PostUserId)
                .ForeignKey("dbo.Service", t => t.ServiceId)
                .Index(t => t.EnrollmentId)
                .Index(t => t.ServiceId)
                .Index(t => t.PostUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnrollService", "ServiceId", "dbo.Service");
            DropForeignKey("dbo.EnrollService", "PostUserId", "dbo.PostUser");
            DropForeignKey("dbo.EnrollService", "EnrollmentId", "dbo.Enrollment");
            DropIndex("dbo.EnrollService", new[] { "PostUserId" });
            DropIndex("dbo.EnrollService", new[] { "ServiceId" });
            DropIndex("dbo.EnrollService", new[] { "EnrollmentId" });
            DropTable("dbo.EnrollService");
        }
    }
}

namespace FYstudentMgr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addenrollservice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Class", "StudentCount", c => c.Int(nullable: false));
            CreateIndex("dbo.Student", "IdCardNO", unique: true, name: "IdCardNOIndex");
            DropColumn("dbo.Class", "IsLock");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Class", "IsLock", c => c.Boolean(nullable: false));
            DropIndex("dbo.Student", "IdCardNOIndex");
            DropColumn("dbo.Class", "StudentCount");
        }
    }
}

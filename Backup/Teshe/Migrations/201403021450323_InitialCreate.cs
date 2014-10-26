namespace Teshe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        Password = c.String(nullable: false, maxLength: 40),
                        RegisterOn = c.DateTime(nullable: false),
                        IsVerify = c.Int(nullable: false),
                        ResponsiblePerson = c.String(nullable: false),
                        Company = c.String(nullable: false),
                        District = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        Email = c.String(),
                        Remarks = c.String(),
                        PhotoUrl = c.String(),
                        UserType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserTypes", t => t.UserType_Id)
                .Index(t => t.UserType_Id);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        BarCode = c.String(nullable: false),
                        PhotoUrl = c.String(),
                        Company = c.String(nullable: false),
                        District = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        ManufactureDate = c.DateTime(nullable: false),
                        Factory = c.String(nullable: false),
                        SetupTime = c.DateTime(nullable: false),
                        ExplosionProof = c.String(nullable: false),
                        SecurityCertificateNo = c.String(nullable: false),
                        CheckState = c.String(nullable: false),
                        CheckTime = c.DateTime(nullable: false),
                        CheckCycle = c.Int(nullable: false),
                        UseState = c.String(nullable: false),
                        MaintenanceRecord = c.String(),
                        Remarks = c.String(),
                        InputTime = c.DateTime(nullable: false),
                        UserInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_Id)
                .Index(t => t.UserInfo_Id);
            
            CreateTable(
                "dbo.Mails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contents = c.String(),
                        SendTime = c.DateTime(nullable: false),
                        IsRead = c.Int(nullable: false),
                        ReceivedUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.ReceivedUser_Id)
                .Index(t => t.ReceivedUser_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mails", new[] { "ReceivedUser_Id" });
            DropIndex("dbo.Devices", new[] { "UserInfo_Id" });
            DropIndex("dbo.UserInfoes", new[] { "UserType_Id" });
            DropForeignKey("dbo.Mails", "ReceivedUser_Id", "dbo.UserInfoes");
            DropForeignKey("dbo.Devices", "UserInfo_Id", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoes", "UserType_Id", "dbo.UserTypes");
            DropTable("dbo.Mails");
            DropTable("dbo.Devices");
            DropTable("dbo.UserTypes");
            DropTable("dbo.UserInfoes");
        }
    }
}

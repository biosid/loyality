using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{
    public partial class Security_Tokens_Service : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OtpTokens",
                c => new
                    {
                        Token = c.String(nullable: false, maxLength: 32),
                        OtpType = c.String(nullable: false, maxLength: 50),
                        Otp = c.String(nullable: false, maxLength: 20),
                        To = c.String(maxLength: 100),
                        ExternalId = c.String(maxLength: 255),
                        MessageTemplate = c.String(nullable: false, maxLength: 255),
                        AttemptsToConfirm = c.Int(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsFake = c.Boolean(nullable: false),
                        ConfirmedTimeUtc = c.DateTime(),
                        CreationTimeUtc = c.DateTime(nullable: false),
                        ExpirationTimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Token);
            
            CreateTable(
                "dbo.SecurityTokens",
                c => new
                    {
                        Token = c.String(nullable: false, maxLength: 32),
                        PrincipalId = c.String(nullable: false, maxLength: 255),
                        ExternalId = c.String(maxLength: 255),
                        CreationTimeUtc = c.DateTime(nullable: false),
                        ExpirationTimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Token);
            
            DropTable("dbo.Tokens");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        OtpToken = c.String(nullable: false, maxLength: 32),
                        OtpType = c.String(nullable: false, maxLength: 50),
                        Otp = c.String(nullable: false, maxLength: 20),
                        To = c.String(maxLength: 100),
                        ExternalId = c.String(maxLength: 255),
                        MessageTemplate = c.String(nullable: false, maxLength: 255),
                        AttemptsToConfirm = c.Int(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsFake = c.Boolean(nullable: false),
                        ConfirmedTimeUtc = c.DateTime(),
                        CreationTimeUtc = c.DateTime(nullable: false),
                        ExpirationTimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OtpToken);
            
            DropTable("dbo.SecurityTokens");
            DropTable("dbo.OtpTokens");
        }
    }
}

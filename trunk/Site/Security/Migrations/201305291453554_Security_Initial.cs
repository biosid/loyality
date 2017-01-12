using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{
    
    public partial class SecurityInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        IsDisabled = c.Boolean(nullable: false),
                        IsPaswordSet = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        OtpToken = c.String(nullable: false, maxLength: 32),
                        To = c.String(maxLength: 100),
                        ExternalId = c.String(maxLength: 255),
                        ClientId = c.String(maxLength: 50),
                        Attempts = c.Int(nullable: false),
                        IsConfired = c.Boolean(nullable: false),
                        ConfirmedTimeUtc = c.DateTime(),
                        CreationTimeUtc = c.DateTime(nullable: false),
                        ExpirationTimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OtpToken);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tokens");
            DropTable("dbo.Users");
        }
    }
}

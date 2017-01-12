namespace Vtb24.Site.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecurityTokenIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("SecurityTokens", new[] { "PrincipalId" }, name: "IX_SecurityTokens_PrincipalId");
        }
        
        public override void Down()
        {
        }
    }
}

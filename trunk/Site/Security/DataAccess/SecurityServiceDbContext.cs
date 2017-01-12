using System.Data.Entity;
using Vtb24.Site.Security.Infrastructure;
using Vtb24.Site.Security.Migrations;
using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Outputs;
using Vtb24.Site.Security.SecurityTokenService.Models;

namespace Vtb24.Site.Security.DataAccess
{
    internal class SecurityServiceDbContext : DbContext, ISecurityDataAccess, IOtpDataAccess, ISecurityTokenDataAccess
    {
        static SecurityServiceDbContext() 
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<SecurityServiceDbContext,Configuration>()
            );   
        }

        public SecurityServiceDbContext() : base(GetConnectionString())
        {
        }

        #region Безопасность

        public DbSet<User> Users { get; set; }

        public DbSet<PhoneNumberChange> PhoneNumberChangeHistory { get; set; }

        public void ResetLastPasswordFailureDate(int userId)
        {
            Database.ExecuteSqlCommand(
                "update dbo.webpages_Membership set LastPasswordFailureDate=NULL from dbo.webpages_Membership where UserId=" +
                userId.ToString("d"));
        }
        
        #endregion

        #region OTP

        public DbSet<OtpToken> Tokens { get; set; }

        #endregion

        #region Токены

        public DbSet<SecurityToken> SecurityTokens { get; set; }

        #endregion Токены

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public static string GetConnectionString()
        {
            return AppSettingsHelper.String("security_connection_string", "SecurityServiceConnection");
        }
    }
}
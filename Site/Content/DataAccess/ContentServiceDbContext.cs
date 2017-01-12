using System.Data.Entity;
using Vtb24.Site.Content.Advertisements.Models;
using Vtb24.Site.Content.Infrastructure;
using Vtb24.Site.Content.Migrations;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Snapshots.Models;
using Vtb24.Site.Content.MyPointImages.Models;

namespace Vtb24.Site.Content.DataAccess
{
    internal class ContentServiceDbContext : DbContext
    {
        public DbSet<NewsMessage> NewsMessages { get; set; }

        public DbSet<DbSnapshot> Snapshots { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<MyPointImage> ImageCatalogs { get; set; }

        public DbSet<ClientAdvertisement> ClientAdvertisements { get; set; }

        public DbSet<Advertisement> Advertisements { get; set; }

        static ContentServiceDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ContentServiceDbContext, Configuration>());
        }

        public ContentServiceDbContext()
            : base(GetConnectionString())
        {
        }

        public static string GetConnectionString()
        {
            return AppSettingsHelper.String("content_connection_string", "ContentServiceConnection");
        }
    }
}

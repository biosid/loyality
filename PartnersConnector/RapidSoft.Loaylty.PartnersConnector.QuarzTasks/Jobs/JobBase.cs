using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using RapidSoft.Loaylty.PartnersConnector.Common.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.Interfaces;

namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    public abstract class JobBase
    {
        public static Func<ICatalogAdminServiceProvider> CatalogAdminServiceProviderBuilder { get; set; }

        public static Func<IFileSystem> FileSystemBuilder { get; set; }

        protected ICatalogAdminServiceProvider GetCatalogAdminServiceProvider()
        {
            var catalogAdminServiceProviderBuilder = CatalogAdminServiceProviderBuilder;

            if (catalogAdminServiceProviderBuilder == null)
            {
                throw new JobExecutionException("Задача не инициализирована корректно", true, true, true);
            }

            var provider = catalogAdminServiceProviderBuilder();

            return provider;
        }

        protected IFileSystem GetFileSystem()
        {
            var fileSystemBuilder = FileSystemBuilder;

            if (fileSystemBuilder == null)
            {
                throw new JobExecutionException("Задача не инициализирована корректно", true, true, true);
            }

            return fileSystemBuilder();
        }
    }
}

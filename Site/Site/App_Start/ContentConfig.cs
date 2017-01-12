using System;
using System.Web.Mvc;
using Vtb24.Site.Content.Pages;

namespace Vtb24.Site.App_Start
{
    public class ContentConfig
    {
        public static void Initialize()
        {
            var pages = DependencyResolver.Current.GetService<IPagesManagement>();
            try
            {
                pages.ReloadBuiltinPagesFromConfiguration();
            }
            catch (Exception)
            {
            }
        }
    }
}
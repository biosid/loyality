using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.App_Start
{
    public class ModelBindersConfig
    {
        public static void RegisterBinders(ModelBinderDictionary bindersDictionary)
        {
            bindersDictionary.Add(typeof (DateTimeRange), new DateTimeRangeBinder());
        }
    }
}
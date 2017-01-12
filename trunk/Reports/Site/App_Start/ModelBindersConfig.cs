using System.Web.Mvc;
using Rapidsoft.VTB24.Reports.Site.Infrastructure;
using Rapidsoft.VTB24.Reports.Site.Models.Shared;

namespace Rapidsoft.VTB24.Reports.Site.App_Start
{
    public static class ModelBindersConfig
    {
        public static void RegisterBinders(ModelBinderDictionary bindersDictionary)
        {
            bindersDictionary.Add(typeof(DateModel), new DateModelBinder());
        }
    }
}

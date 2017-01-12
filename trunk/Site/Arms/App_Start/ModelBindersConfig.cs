using System;
using System.Web.Mvc;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.App_Start
{
    public static class ModelBindersConfig
    {
        public static void RegisterBinders(ModelBinderDictionary bindersDictionary)
        {
            bindersDictionary.Add(typeof(DateTime?), new DateTimeModelBinder());
            bindersDictionary.Add(typeof(decimal?), new DecimalModelBinder());
            bindersDictionary.Add(typeof(decimal), new DecimalModelBinder());
        }
    }
}

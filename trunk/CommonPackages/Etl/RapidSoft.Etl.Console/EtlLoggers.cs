//using System;
//using System.ComponentModel;
//using System.Xml.Serialization;

//using RapidSoft.Etl.Logging;

//namespace RapidSoft.Etl.Console
//{
//    public static class EtlLoggers
//    {
//        #region Methods
        
//        public static IEtlLogger GetLogger(EtlLoggerInfo loggerInfo)
//        {
//            if (String.IsNullOrEmpty(loggerInfo.LoggerType))
//            {
//                throw new ArgumentNullException("loggerType");
//            }

//            var loggerType = Type.GetType(loggerInfo.LoggerType);
//            if (loggerType == null)
//            {
//                throw new InvalidOperationException(string.Format("Type {0} was not found", loggerInfo.LoggerType));
//            }

//            if (!typeof(IEtlLogger).IsAssignableFrom(loggerType))
//            {
//                throw new InvalidOperationException(string.Format("Type {0} must implements {1} interface", loggerType.FullName, typeof(IEtlLogger).FullName));
//            }

//            var ctorWithStrings = loggerType.GetConstructor(new[] { typeof(string), typeof(string) });
//            var ctorWithoutParams = loggerType.GetConstructor(Type.EmptyTypes);

//            if (ctorWithStrings != null)
//            {
//                return (IEtlLogger)ctorWithStrings.Invoke(new object[] { loggerInfo.ConnectionString, loggerInfo.SchemaName });
//            }
//            else if (ctorWithoutParams != null)
//            {
//                return (IEtlLogger)ctorWithoutParams.Invoke(null);
//            }
//            else
//            {
//                throw new InvalidOperationException(string.Format("Logger fabric does not support type \"{0}\"", loggerType.FullName));
//            }
//        }

//        #endregion
//    }
//}
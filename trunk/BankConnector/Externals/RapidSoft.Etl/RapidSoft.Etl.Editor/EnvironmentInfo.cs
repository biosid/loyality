using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace RapidSoft.Etl.Editor
{
    [Serializable]
    public class EnvironmentInfo
    {
        #region Properties

        public string ProductName
        {
            get;
            set;
        }

        public string AppVersion
        {
            get;
            set;
        }

        public string AppDataPath
        {
            get;
            set;
        }

        public string Thread_CurrentPrincipal_Identity_Name
        {
            get;
            set;
        }

        public string Thread_CurrentPrincipal_Identity_AuthenticationType
        {
            get;
            set;
        }

        public string Environment_UserDomainName
        {
            get;
            set;
        }

        public string Environment_UserName
        {
            get;
            set;
        }

        public string Environment_OSVersion_VersionString
        {
            get;
            set;
        }

        public string Environment_CurrentDirectory
        {
            get;
            set;
        }

        public string ExecutingAssembly_FullName
        {
            get;
            set;
        }

        public string Thread_CurrentThread_CurrentCulture_Name
        {
            get;
            set;
        }

        public string Thread_CurrentThread_CurrentUICulture_Name
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public static EnvironmentInfo GetEnvironmentInfo()
        {
            var info = new EnvironmentInfo
            {
                ProductName = Application.ProductName,
                AppVersion = Application.ProductVersion,
                Thread_CurrentPrincipal_Identity_Name = Thread.CurrentPrincipal.Identity.Name,
                Thread_CurrentPrincipal_Identity_AuthenticationType = Thread.CurrentPrincipal.Identity.AuthenticationType,
                Environment_UserDomainName = Environment.UserDomainName,
                Environment_UserName = Environment.UserName,
                Environment_OSVersion_VersionString = Environment.OSVersion.VersionString,
                Environment_CurrentDirectory = Environment.CurrentDirectory,
                ExecutingAssembly_FullName = Assembly.GetExecutingAssembly().FullName,
                AppDataPath = GetAppDataPath(),
                Thread_CurrentThread_CurrentCulture_Name = Thread.CurrentThread.CurrentCulture.Name,
                Thread_CurrentThread_CurrentUICulture_Name = Thread.CurrentThread.CurrentUICulture.Name,
            };

            if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null)
            {
                info.Thread_CurrentPrincipal_Identity_Name = Thread.CurrentPrincipal.Identity.Name;
                info.Thread_CurrentPrincipal_Identity_AuthenticationType = Thread.CurrentPrincipal.Identity.AuthenticationType;
            }

            return info;
        }

        public static string GetAppDataPath()
        {
            var path = Path.Combine
            (
                Path.Combine
                (
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Application.CompanyName
                ),
                Application.ProductName
            );

            return path;
        }

        #endregion
    }
}
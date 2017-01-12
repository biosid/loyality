namespace RapidSoft.VTB24.ArmSecurity
{
    using System.Configuration;

    public class ActiveDirectoryConfigSection : ConfigurationSection
    {
        public static readonly ActiveDirectoryConfigSection Current =
            (ActiveDirectoryConfigSection)ConfigurationManager.GetSection("activeDirectory");

        [ConfigurationProperty("connection")]
        public ConnectionElement Connection
        {
            get { return (ConnectionElement)base["connection"]; }
            set { base["connection"] = value; }
        }

        public class ConnectionElement : ConfigurationElement
        {
            [ConfigurationProperty("path", IsRequired = true, IsKey = true)]
            public string Path
            {
                get { return (string)base["path"]; }
                set { base["path"] = value; }
            }

            [ConfigurationProperty("username", IsRequired = true)]
            public string UserName
            {
                get { return (string)base["username"]; }
                set { base["username"] = value; }
            }

            [ConfigurationProperty("password", IsRequired = true)]
            public string Password
            {
                get { return (string)base["password"]; }
                set { base["password"] = value; }
            }
        }
    }
}

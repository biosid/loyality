using System;
using System.Collections.Generic;

namespace RapidSoft.Etl.Console
{
    [Serializable]
    public sealed class EtlConsoleCommandOptionInfo
    {
        #region Constructors

        public EtlConsoleCommandOptionInfo()
        {
        }

        public EtlConsoleCommandOptionInfo(string optionName, string description)
            : this(optionName, description, false)
        {
        }

        public EtlConsoleCommandOptionInfo(string optionName, string description, bool optional)
        {
            this.OptionName = optionName;
            this.Description = description;
            this.Optional = optional;
        }

        #endregion

        #region Properties

        public string OptionName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Optional
        {
            get;
            set;
        }

        #endregion
    }
}

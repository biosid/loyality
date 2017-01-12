using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace RapidSoft.Etl.Console
{
    public sealed class EtlConsoleArguments
    {
        #region Fields

        private readonly Dictionary<string, string> _commandOptions = new Dictionary<string, string>();

        #endregion

        #region Properties

        public string CommandName
        {
            get;
            set;
        }

        public Dictionary<string, string> CommandOptions
        {
            [DebuggerStepThrough]
            get
            {
                return _commandOptions;
            }
        }

        public bool InteractiveMode
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public string GetCommandOptionOrNull(string optionName)
        {
            string value;
            if (_commandOptions.TryGetValue(optionName, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public string GetSingleOptionOrNull()
        {
            if (_commandOptions.Count == 1)
            {
                foreach (var value in _commandOptions.Values)
                {
                    return value;
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}

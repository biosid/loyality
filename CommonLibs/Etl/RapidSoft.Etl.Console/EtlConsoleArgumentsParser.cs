using System;
using System.Collections.Generic;

namespace RapidSoft.Etl.Console
{
    /// <summary>
    /// CommandLineArguments class. accepts supportedCommandName line in the following forms:
    /// app.exe param1 /option1 /option2:v2 /option3:"str 3"
    /// 
    /// Note: class uses case-insensetive notation, i.e  /option and /OPTION will be the same
    /// </summary>
    internal static class EtlConsoleArgumentsParser
    {
        #region Fields

        public static readonly string InteractiveModeOption = "interactive";
        public static readonly string OptionPrefix = "/";
        public static readonly string OptionValueDelimiter = ":";

        #endregion

        #region Methods

        public static EtlConsoleArguments Parse(string[] commandLineArgs)
        {
            var options = new EtlConsoleArguments();

            if (commandLineArgs == null || commandLineArgs.Length == 0)
            {
                return options;
            }

            int firstOptionIndex;

            var possibleCommandName = commandLineArgs[0] != null ? commandLineArgs[0].Trim() : null;
            if (possibleCommandName != null && possibleCommandName.StartsWith(OptionPrefix))
            {
                firstOptionIndex = 0;
            }
            else
            {
                firstOptionIndex = 1;
                options.CommandName = possibleCommandName;
            }

            for (var i = firstOptionIndex; i < commandLineArgs.Length; i++)
            {
                var arg = commandLineArgs[i];
                if (string.IsNullOrEmpty(arg))
                {
                    continue;
                }

                if (arg[0] != OptionPrefix[0])
                {
                    options.CommandOptions.Add(arg.ToLower(), null);
                }
                else
                {
                    arg = arg.Substring(1);
                    var argArr = arg.Split(OptionValueDelimiter.ToCharArray(), 2);
                    options.CommandOptions.Add(argArr[0].ToLower(), ((argArr.Length > 1) && (argArr[1].Length > 0)) ? argArr[1] : null);
                }
            }

            if (options.CommandOptions.ContainsKey(InteractiveModeOption))
            {
                options.InteractiveMode = true;
                options.CommandOptions.Remove(InteractiveModeOption);
            }

            return options;
        }

        #endregion
    }
}

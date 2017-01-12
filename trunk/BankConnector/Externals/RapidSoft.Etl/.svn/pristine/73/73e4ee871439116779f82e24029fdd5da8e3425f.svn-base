using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace RapidSoft.Etl.Console
{
    public sealed class EtlHelpCommand : IEtlConsoleCommand
    {
        #region Constants

        private const string COMMAND_NAME = "help";
        private const string PARAM_NAME_COMMAND = "command";

        #endregion

        #region Fields

        private static readonly string _delimiterLine = new String('-', 50);

        #endregion

        #region Properties

        public string CommandName
        {
            get
            {
                return COMMAND_NAME;
            }
        }

        public string ShortDescription
        {
            get
            {
                //todo: localize message
                return @"displays help on specified command (e.g. ""help /command:help"")";
            }
        }

        public string Description
        {
            get
            {
                //todo: localize message
                return @"Displays help on specified command";
            }
        }

        public EtlConsoleCommandOptionInfo[] OptionsInfo
        {
            get
            {
                return new[]
                {
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_COMMAND, "name_of_command_to_help")
                };
            }
        }

        #endregion

        #region Methods

        public void ExecuteCommand(EtlConsoleArguments options)
        {
            var commandNameToHelp = options.GetCommandOptionOrNull(PARAM_NAME_COMMAND);
            if (commandNameToHelp == null)
            {
                ShowAllHelp();
            }
            else
            {
                ShowHelp(commandNameToHelp);
            }
        }

        public static void ShowHelp(string commandNameToHelp)
        {
            var command = FindCommandByName(commandNameToHelp);
            if (command == null)
            {
                //todo: localize message
                System.Console.WriteLine("Command \"{0}\" not found", commandNameToHelp);
                return;
            }

            System.Console.Write(command.CommandName.ToUpper());
            System.Console.WriteLine();
            System.Console.Write(command.Description);
            System.Console.Write(".");
            System.Console.WriteLine();

            System.Console.Write("Syntax: ");
            System.Console.Write(command.CommandName);

            var optionsInfo = command.OptionsInfo;

            if (optionsInfo != null && optionsInfo.Length > 0)
            {
                foreach (var optionInfo in optionsInfo)
                {
                    System.Console.Write(" ");

                    if (optionInfo.Optional)
                    {
                        System.Console.Write("[");
                    }

                    System.Console.Write(EtlConsoleArgumentsParser.OptionPrefix);
                    System.Console.Write(optionInfo.OptionName);
                    System.Console.Write(EtlConsoleArgumentsParser.OptionValueDelimiter);
                    System.Console.Write("<");
                    System.Console.Write(optionInfo.Description);
                    System.Console.Write(">");

                    if (optionInfo.Optional)
                    {
                        System.Console.Write("]");
                    }
                }
            }

            System.Console.WriteLine();
        }

        public static void ShowAllHelp()
        {
            var commandsByNames = GetCommandsByNames();
            var commandNumberLength = commandsByNames.Count.ToString().Length;

            var n = 1;
            foreach (var commandByName in commandsByNames)
            {
                var command = commandByName.Value;

                System.Console.Write(command.CommandName.ToUpper());
                System.Console.Write(" - ");
                System.Console.Write(command.ShortDescription);
                System.Console.Write(";");
                System.Console.WriteLine();

                n++;
            }
        }

        private static SortedList<string, IEtlConsoleCommand> GetCommandsByNames()
        {
            var commands = EtlConsoleCommandProvider.GetCommands();
            var commandsByNames = new SortedList<string, IEtlConsoleCommand>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var command in commands)
            {
                commandsByNames.Add(command.CommandName, command);
            }

            return commandsByNames;
        }

        private static IEtlConsoleCommand FindCommandByName(string commandName)
        {
            var commands = EtlConsoleCommandProvider.GetCommands();
            foreach (var command in commands)
            {
                if (string.Equals(command.CommandName, commandName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return command;
                }
            }

            return null;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RapidSoft.Etl.Console
{
    class Program
    {
        #region Fields

        private static readonly string _delimiterLine = new String('=', 50);

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            //todo: localize message
            System.Console.WriteLine("RapidSoft.Etl.Console started at {0}", DateTime.Now);
            System.Console.WriteLine(_delimiterLine);

            Main1(EtlConsoleArgumentsParser.Parse(args));
        }

        private static void Main1(EtlConsoleArguments args)
        {
            //todo: throw exception on unknown args

            try
            {
                if (string.IsNullOrEmpty(args.CommandName))
                {
                    EtlHelpCommand.ShowAllHelp();
                }
                else
                {
                    var commandHandler = EtlConsoleCommandProvider.FindCommand(args.CommandName);
                    if (commandHandler == null)
                    {
                        System.Console.WriteLine("Command \"{0}\" is not supported.", args.CommandName);
                        EtlHelpCommand.ShowAllHelp();
                    }
                    else
                    {
                        commandHandler.ExecuteCommand(args);
                    }
                }

                System.Console.WriteLine(_delimiterLine);
                System.Console.WriteLine("RapidSoft.Etl.Console finished at {0}", DateTime.Now);
            }
            catch (Exception e)
            {
                var sb = new StringBuilder(String.Format("Console parameters:{0}", Environment.NewLine));
                foreach (var param in args.CommandOptions.Keys)
                {
                    sb.AppendFormat("{0}={1}{2}", param, args.CommandOptions[param], Environment.NewLine);
                }

                sb.Append(Environment.NewLine);
                sb.Append(e.ToString());

                System.Console.WriteLine(sb.ToString());

                if (Config.EventLogEnabled)
                {
                    if (!EventLog.SourceExists(Config.EventSourceName))
                    {
                        try
                        {
                            EventLog.CreateEventSource(Config.EventSourceName, Config.EventLogName);
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine(String.Format("Unable to create event log{0}{1}", Environment.NewLine, ex.ToString()));
                        }
                    }

                    if (EventLog.SourceExists(Config.EventSourceName))
                    {
                        EventLog eventLog = new EventLog();
                        eventLog.Source = Config.EventSourceName;
                        eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Error);
                    }
                    else
                    {
                        System.Console.WriteLine("Event log {0} doesnt exist", Config.EventSourceName);
                    }
                }
            }

            if (args.InteractiveMode)
            {
                System.Console.WriteLine("Press any key to exit");
                System.Console.ReadKey();
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace RapidSoft.Etl.Console
{
    internal static class EtlConsoleCommandProvider
    {
        #region Methods

        public static IEtlConsoleCommand FindCommand(string commandName)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException("commandName");
            }

            var command = GetCommands();
            IEtlConsoleCommand foundCommand = null;
            foreach (var handler in command)
            {
                if (IsSupportedCommandName(handler, commandName))
                {
                    if (foundCommand != null)
                    {
                        throw new InvalidOperationException(string.Format("Too many handlers for command \"{0}\" found", commandName));
                    }

                    foundCommand = handler;
                }
            }

            return foundCommand;
        }
        
        public static IEtlConsoleCommand[] GetCommands()
        {
            var types = GetCommandTypes();
            var commands = new IEtlConsoleCommand[types.Length];
            for (var i = 0; i < commands.Length; i++)
            {
                var ctor = types[i].GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                {
                    throw new InvalidOperationException(string.Format("{0} must have parameterless constructor", types[i].FullName));
                }

                commands[i] = (IEtlConsoleCommand)ctor.Invoke(null);
            }

            return commands;
        }

        private static bool IsSupportedCommandName(IEtlConsoleCommand command, string commandName)
        {
            return string.Equals(command.CommandName, commandName, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Type[] GetCommandTypes()
        {
            var commandTypes = new List<Type>();

            foreach (var type in typeof(IEtlConsoleCommand).Assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && typeof(IEtlConsoleCommand).IsAssignableFrom(type))
                {
                    commandTypes.Add(type);
                }
            }

            return commandTypes.ToArray();
        }

        #endregion
    }
}

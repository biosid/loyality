using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime;
using RapidSoft.Etl.Runtime.Agents;

namespace RapidSoft.Etl.Console
{
    public sealed class EtlInvokeCommand : IEtlConsoleCommand
    {
        #region Constants

        private const string COMMAND_NAME = "invoke";
        private const string PARAM_NAME_AGENT_TYPE = "agent_type";
        private const string PARAM_NAME_AGENT_CONNECTION_STRING = "agent_connectionstring";
        private const string PARAM_NAME_AGENT_SCHEMA = "agent_schema";
        private const string PARAM_NAME_PACKAGE_ID = "packageid";
        private const string PARAM_NAME_VARIABLES = "variables";

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
                return "invokes ETL package using command line arguments";
            }
        }

        public string Description
        {
            get
            {
                //todo: localize message
                return "Invokes ETL package using command line arguments";
            }
        }

        public EtlConsoleCommandOptionInfo[] OptionsInfo
        {
            get
            {
                return new[]
                {
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_AGENT_TYPE, "type of ETL agent"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_AGENT_CONNECTION_STRING, "ETL agent connection string"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_AGENT_SCHEMA, "ETL agent schema", true),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_PACKAGE_ID, "identifier of ETL package to invoke"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_VARIABLES, "ETL package variables formatted as \"variable1='value1';variable2='value2';variable3='value3';\""),
                };
            }
        }

        #endregion

        #region Methods

        //todo: localize log message
        public void ExecuteCommand(EtlConsoleArguments options)
        {
            System.Console.WriteLine("Verifying input parameters...");

            var errorMsg = VerifyArguments(options);

            if (!String.IsNullOrEmpty(errorMsg))
            {
                throw new Exception(String.Format("Input parameters incorrect: {0}", errorMsg));
            }

            var parameters = ParseParameters(options.GetCommandOptionOrNull(PARAM_NAME_VARIABLES));

            System.Console.WriteLine("Input parameters are correct");

            System.Console.WriteLine("Creating ETL agent...");
            var agentInfo = new EtlAgentInfo()
            {
                EtlAgentType = options.CommandOptions[PARAM_NAME_AGENT_TYPE],
                ConnectionString = options.CommandOptions[PARAM_NAME_AGENT_CONNECTION_STRING],
                SchemaName = options.CommandOptions.ContainsKey(PARAM_NAME_AGENT_SCHEMA) ? options.CommandOptions[PARAM_NAME_AGENT_SCHEMA] : String.Empty,
            };

            var agent = EtlAgents.CreateAgent(agentInfo);
            if (agent is ILocalEtlAgent)
            {
                ((ILocalEtlAgent)agent).AttachLogger(new ConsoleEtlLogger(System.Console.Out));
            }

            System.Console.WriteLine("ETL agent created");

            System.Console.WriteLine("Invoking package...");
            var result = agent.InvokeEtlPackage(options.CommandOptions[PARAM_NAME_PACKAGE_ID], parameters, null);
            System.Console.WriteLine(string.Format("Package has been executed with result {0}", result.Status));
        }

        private string VerifyArguments(EtlConsoleArguments options)
        {
            var sb = new StringBuilder();

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_AGENT_TYPE) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_AGENT_TYPE]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_AGENT_TYPE);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_AGENT_CONNECTION_STRING) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_AGENT_CONNECTION_STRING]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_AGENT_CONNECTION_STRING);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_PACKAGE_ID) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_PACKAGE_ID]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_PACKAGE_ID);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_VARIABLES))
            {
                sb.AppendFormat("{0};", PARAM_NAME_VARIABLES);
            }

            return sb.ToString();
        }

        private static EtlVariableAssignment[] ParseParameters(string str)
        {
            if (str == null)
            {
                return new EtlVariableAssignment[0];
            }

            str = str.Trim();
            if (str == "")
            {
                return new EtlVariableAssignment[0];
            }

            if (!str.EndsWith(";"))
            {
                str = str + ";";
            }

            var parameters = new List<EtlVariableAssignment>();
            var buf = new StringBuilder();
            var isVariableName = true;
            var wasQuote = false;
            var wasEscape = false;
            var parameter = new EtlVariableAssignment();

            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];

                if (ch == '=')
                {
                    wasEscape = false;

                    if (wasQuote)
                    {
                        buf.Append(ch);
                    }
                    else if (isVariableName)
                    {
                        parameter.Name = buf.ToString();
                        buf.Length = 0;
                        isVariableName = false;
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else if (ch == '\\')
                {
                    if (wasQuote && str[i + 1] == '\'')
                    {
                        wasEscape = true;
                    }
                    else
                    {
                        wasEscape = false;
                        buf.Append(ch);
                    }
                }
                else if (ch == '\'')
                {
                    if (isVariableName)
                    {
                        throw new FormatException();
                    }

                    if (wasEscape)
                    {
                        buf.Append(ch);
                    }
                    else
                    {
                        wasQuote = !wasQuote;
                    }
                }
                else if (ch == ';')
                {
                    wasEscape = false;

                    if (wasQuote)
                    {
                        buf.Append(ch);
                    }
                    else
                    {
                        if (isVariableName)
                        {
                            parameter.Name = buf.ToString();
                        }
                        else
                        {
                            parameter.Value = buf.ToString();
                        }

                        parameters.Add(parameter);
                        parameter = new EtlVariableAssignment();
                        
                        buf.Length = 0;
                        isVariableName = true;
                    }
                }
                else
                {
                    wasEscape = false;
                    buf.Append(ch);
                }
            }

            return parameters.ToArray();
        }

        #endregion
    }
}
using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Editor
{
    public class MessageFormData
    {
        #region Properties

        public MessageFormType MessageType
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string DisplayText
        {
            get;
            set;
        }

        public ExceptionInfo ExceptionInfo
        {
            get;
            set;
        }

        public EnvironmentInfo EnvironmentInfo
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public static void SerializeToText(MessageFormData messageData, StringBuilder sb)
        {
            MessageFormDataTextSerializer.SerializeToText(messageData, sb);
        }

        public static string SerializeToText(MessageFormData messageData)
        {
            var sb = new StringBuilder();
            MessageFormDataTextSerializer.SerializeToText(messageData, sb);

            return sb.ToString();
        }

        public static void SerializeToShortText(MessageFormData messageData, StringBuilder sb)
        {
            MessageFormDataShortTextSerializer.SerializeToShortText(messageData, sb);
        }

        public static string SerializeToShortText(MessageFormData messageData)
        {
            var sb = new StringBuilder();
            MessageFormDataShortTextSerializer.SerializeToShortText(messageData, sb);

            return sb.ToString();
        }

        public static void SerializeToXml(MessageFormData data, StringBuilder sb)
        {
            if (data == null)
            {
                return;
            }

            var writer = new StringWriter(sb);
            var serializer = new XmlSerializer(typeof(MessageFormData));
            serializer.Serialize(writer, data);
        }

        public static string SerializeToXml(MessageFormData data)
        {
            if (data == null)
            {
                return "";
            }

            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(MessageFormData));
            serializer.Serialize(writer, data);

            return writer.ToString();
        }

        public override string ToString()
        {
            return this.DisplayText ?? "";
        }

        #endregion

        #region Nested classes

        private static class MessageFormDataTextSerializer
        {
            #region Constants

            private const string DELIMITER_LINE = "------------------------------";

            #endregion

            public static void SerializeToText(MessageFormData messageData, StringBuilder sb)
            {
                if (messageData == null)
                {
                    return;
                }

                sb.Append(DELIMITER_LINE);
                sb.AppendLine();
                sb.Append(messageData.Title);
                sb.AppendLine();
                sb.Append(DELIMITER_LINE);
                sb.AppendLine();
                sb.Append(messageData.DisplayText);
                sb.AppendLine();
                sb.Append(DELIMITER_LINE);
                sb.AppendLine();
                sb.AppendLine();
                sb.Append("Техническая информация:");
                sb.AppendLine();
                MessageFormData.SerializeToXml(messageData, sb);
                sb.AppendLine();
                sb.Append(DELIMITER_LINE);
                sb.AppendLine();
            }
        }

        private static class MessageFormDataShortTextSerializer
        {
            #region Constants

            private const string NULL_TEXT = "null";
            private const string EMPTY_TEXT = "empty";

            private const string SECTION_DELIMITER = "=====";
            private const char ESCAPE_CHAR = '\\';

            private const string FIELD_LP = "{";
            private const string FIELD_RP = "}";
            private const string FIELD_ESCAPED_RP = "\\}";

            private const int MAX_CONTEXT_VALUES_COUNT = 3;
            private const int MAX_CONTEXT_VALUE_LENGTH = 50;

            #endregion

            #region Methods

            public static void SerializeToShortText(MessageFormData messageData, StringBuilder sb)
            {
                if (messageData == null)
                {
                    return;
                }

                sb.AppendLine(SerializeValue(messageData.DisplayText));
                sb.AppendLine(SECTION_DELIMITER);

                sb.AppendLine("Техническая информация:");
                SerializeShortEnvironmentInfoToStringBuilder(messageData.EnvironmentInfo, sb);
                SerializeShortExceptionInfoToStringBuilder(messageData.ExceptionInfo, sb);
            }

            private static void SerializeShortEnvironmentInfoToStringBuilder(EnvironmentInfo info, StringBuilder sb)
            {
                if (info == null)
                {
                    return;
                }

                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.Thread_CurrentPrincipal_Identity_Name));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.AppVersion));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.AppDataPath));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.Environment_CurrentDirectory));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.Thread_CurrentThread_CurrentCulture_Name));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.Thread_CurrentThread_CurrentUICulture_Name));
                sb.Append(FIELD_RP);
            }

            private static void SerializeShortExceptionInfoToStringBuilder(ExceptionInfo info, StringBuilder sb)
            {
                if (info == null)
                {
                    return;
                }

                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.Message));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.ExceptionTypeName));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                sb.Append(SerializeValue(info.LocalDateTime));
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                SerializeContextValues(sb, info.ContextValues);
                sb.Append(FIELD_RP);
                sb.Append(FIELD_LP);
                SerializeStackTrace(sb, info.StackTrace);
                sb.Append(FIELD_RP);
            }

            private static string SerializeValue(string value)
            {
                if (value == null)
                {
                    return NULL_TEXT;
                }
                else
                {
                    value = value.Trim();

                    if (value == "")
                    {
                        return EMPTY_TEXT;
                    }

                    value = value.Replace(FIELD_RP, FIELD_ESCAPED_RP);
                    return value;
                }
            }

            private static void SerializeStackTrace(StringBuilder sb, string stackTrace)
            {
                stackTrace = SerializeValue(stackTrace);
                stackTrace = stackTrace.Replace(Environment.NewLine, " ");
                stackTrace = stackTrace.Replace("  ", " ");
                stackTrace = stackTrace.Replace("   ", " ");

                sb.Append(stackTrace);
            }

            private static void SerializeContextValues(StringBuilder sb, string[] contextValues)
            {
                if (contextValues == null)
                {
                    return;
                }

                for (var i = 0; i < contextValues.Length; i++)
                {
                    if (i > MAX_CONTEXT_VALUES_COUNT)
                    {
                        break;
                    }

                    var contextValue = contextValues[i];

                    if (contextValue == null)
                    {
                        contextValue = "";
                    }
                    else
                    {
                        if (contextValue.Length > MAX_CONTEXT_VALUE_LENGTH)
                        {
                            contextValue = contextValue.Substring(0, MAX_CONTEXT_VALUE_LENGTH);
                        }
                    }

                    sb.Append(SerializeValue(contextValue));
                }
            }

            #endregion
        }

        #endregion
    }
}
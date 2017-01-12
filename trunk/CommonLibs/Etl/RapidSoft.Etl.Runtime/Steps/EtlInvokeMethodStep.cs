using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.DataSources.Xml;
using RapidSoft.Etl.Runtime.DataSources.DB;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlInvokeMethodStep : EtlStep
    {
        #region Constructors

        public EtlInvokeMethodStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlMethodSourceInfo Source
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (this.Source == null)
            {
                throw new InvalidOperationException("Source cannot be null");
            }

            if (string.IsNullOrEmpty(this.Source.AssemblyName))
            {
                throw new InvalidOperationException("Source.Assembly cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.TypeName))
            {
                throw new InvalidOperationException("Source.TypeName cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.MethodName))
            {
                throw new InvalidOperationException("Source.MethodName cannot be empty");
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);
            var sourcePath = string.Format("{0}, {1}, {2}", this.Source.AssemblyName, this.Source.TypeName, this.Source.MethodName);

            var asm = Assembly.Load(this.Source.AssemblyName);
            
            var type = asm.GetType(this.Source.TypeName);
            if (type == null)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.InvokeMethodCannotFindType, this.Source.AssemblyName, this.Source.TypeName));
            }

            var method = type.GetMethod(this.Source.MethodName);
            if (!method.IsStatic)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.InvokeMethodCannotInvokeNonStaticMethod, this.Source.AssemblyName, this.Source.TypeName, this.Source.MethodName));
            }

            var values = GetParameterValues(method, this.Source.Parameters, context, logger);
            var res = method.Invoke(null, values);

            if (res != null && res is EtlStepResult)
            {
                return res as EtlStepResult;
            }

            return result;
        }

        private static object[] GetParameterValues(MethodInfo method, IEnumerable<EtlMethodParameter> parameters, EtlContext context, IEtlLogger logger)
        { 
            var parametersInfo = method.GetParameters();
            var values = new object[parametersInfo.Length];

            for (var i = 0; i < parametersInfo.Length; i++)
            {
                var parameterInfo = parametersInfo[i];
                var parameter = FindParameter(parameters, parameterInfo.Name);
                if (parameter == null)
                {
                    values[i] = GetSpecialValue(parameterInfo, context, logger);
                }
                else
                {
                    values[i] = EtlValueConverter.ParseType(parameter.Value, Type.GetTypeCode(parameterInfo.ParameterType));
                }
            }

            return values;
        }

        private static object GetSpecialValue(ParameterInfo parameterInfo, EtlContext context, IEtlLogger logger)
        {
            if (parameterInfo.ParameterType == typeof(IEtlLogger))
            {
                return logger;
            }
            else if (parameterInfo.ParameterType == typeof(EtlContext))
            {
                return context;
            }
            else
            {
                return null;
            }
        }

        private static EtlMethodParameter FindParameter(IEnumerable<EtlMethodParameter> parameters, string parameterName)
        {
            foreach (var parameter in parameters)
            {
                if (string.Equals(parameter.Name, parameterName, StringComparison.InvariantCulture))
                {
                    return parameter;
                }
            }

            return null;
        }

        #endregion
    }
}

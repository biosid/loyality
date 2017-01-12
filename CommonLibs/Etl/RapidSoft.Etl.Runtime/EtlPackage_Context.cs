using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using System.Threading;

using RapidSoft.Etl.Logging;
using System.Reflection;

namespace RapidSoft.Etl.Runtime
{
    partial class EtlPackage
    {
        private sealed class _Context : EtlContext
        {
            #region Constructors

            public _Context(EtlPackage originalPackage, EtlSession session)
                : base(session)
            {
                _currentStatus = session.Status;
                _originalPackage = originalPackage;
            }

            #endregion

            #region Fields

            private readonly EtlPackage _originalPackage;
            private readonly List<EtlVariable> _variables = new List<EtlVariable>();

            private EtlStatus _currentStatus;

            #endregion

            #region Properties

            public EtlPackage OriginalPackage
            {
                [DebuggerStepThrough]
                get
                {
                    return _originalPackage;
                }
            }

            public EtlStatus CurrentStatus
            {
                [DebuggerStepThrough]
                get
                {
                    return _currentStatus;
                }
                [DebuggerStepThrough]
                set
                {
                    _currentStatus = value;
                }
            }

            #endregion

            #region Methods

            public EtlVariable[] GetVariables()
            {
                var variables = new EtlVariable[_variables.Count];
                for (var i = 0; i < variables.Length; i++)
                {
                    variables[i] = (EtlVariable)_variables[i].Clone();
                }

                return variables;
            }

            public void InitVariables(IEnumerable<EtlVariableInfo> variablesInfo, IEnumerable<EtlVariableAssignment> assignments)
            {
                _variables.Clear();

                foreach (var variableInfo in variablesInfo)
                {
                    var variable = new EtlVariable
                    {
                        EtlPackageId = this.EtlPackageId,
                        EtlSessionId = this.EtlSessionId,
                        Name = variableInfo.Name,
                        Modifier = variableInfo.Modifier,
                        IsSecure = variableInfo.IsSecure,
                        DateTime = DateTime.Now,
                        UtcDateTime = DateTime.Now.ToUniversalTime(),
                    };

                    switch (variableInfo.Modifier)
                    {
                        case EtlVariableModifier.Bound:
                            variable.Value = EvaluateBoundVariable(variableInfo);
                            break;
                        case EtlVariableModifier.Input:
                            variable.Value = variableInfo.DefaultValue;
                            break;
                        case EtlVariableModifier.Output:
                            variable.Value = variableInfo.DefaultValue;
                            break;
                        default:
                            throw new InvalidOperationException(string.Format(Properties.Resources.UnknownVariableModifier, variableInfo.Name, variableInfo.Modifier));
                    }

                    _variables.Add(variable);
                }

                foreach (var assignment in assignments)
                {
                    AssignVariable(assignment, true);
                }
            }

            public EtlVariable AssignVariable(EtlVariableAssignment assignment)
            {
                return AssignVariable(assignment, false);
            }

            private EtlVariable AssignVariable(EtlVariableAssignment assignment, bool initializing)
            {
                if (assignment == null)
                {
                    throw new ArgumentNullException("assignment");
                }

                if (assignment.Name == null || assignment.Name.Trim() == "")
                {
                    throw new InvalidOperationException(string.Format(Properties.Resources.VariableNameCannotBeEmpty));
                }

                var variableIndex = FindVariableIndex(assignment.Name);
                if (variableIndex < 0)
                {
                    if (initializing)
                    {
                        return null;
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format(Properties.Resources.VariableNotFound, assignment.Name));
                    }
                }

                var variable =_variables[variableIndex];

                if (variable.Modifier == EtlVariableModifier.Bound)
                {
                    throw new InvalidOperationException(string.Format(Properties.Resources.CannotAssignBoundVariable, variable.Name));
                }
                else if (variable.Modifier == EtlVariableModifier.Input && !initializing)
                {
                    throw new InvalidOperationException(string.Format(Properties.Resources.CannotAssignInputVariable, variable.Name));
                }

                var newVariable = (EtlVariable)variable.Clone();
                newVariable.Value = assignment.Value;
                newVariable.DateTime = DateTime.Now;
                newVariable.UtcDateTime = DateTime.Now.ToUniversalTime();
                _variables[variableIndex] = newVariable;

                return newVariable;
            }

            private int FindVariableIndex(string name)
            {
                for (var i = 0; i < _variables.Count; i++)
                {
                    if (string.Equals(_variables[i].Name, name, StringComparison.InvariantCulture))
                    {
                        return i;
                    }
                }

                return -1;
            }

            private string EvaluateBoundVariable(EtlVariableInfo variableInfo)
            {
                switch (variableInfo.Binding)
                {
                    case EtlVariableBinding.None:
                    case EtlVariableBinding.Obsolete_Value:
                    case EtlVariableBinding.Obsolete_String:
                        return null;

                    case EtlVariableBinding.EtlPackageId:
                        return this.EtlPackageId;

                    case EtlVariableBinding.EtlSessionId:
                        return this.EtlSessionId;

                    case EtlVariableBinding.ParentEtlSessionId:
                        return this.ParentEtlSessionId;

                    case EtlVariableBinding.UserName:
                        return this.UserName;

                    case EtlVariableBinding.EtlSessionDate:
                        return GetISODate(this.StartDateTime);

                    case EtlVariableBinding.EtlSessionDateTime:
                        return GetISODateTime(this.StartDateTime);

                    case EtlVariableBinding.EtlSessionYear:
                        return this.StartDateTime.Year.ToString();

                    case EtlVariableBinding.EtlSessionYear4:
                        return this.StartDateTime.Year.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionMonth:
                        return this.StartDateTime.Month.ToString();

                    case EtlVariableBinding.EtlSessionMonth2:
                        return this.StartDateTime.Month.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionDay:
                        return this.StartDateTime.Day.ToString();

                    case EtlVariableBinding.EtlSessionDay2:
                        return this.StartDateTime.Day.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionHour:
                        return this.StartDateTime.Hour.ToString();

                    case EtlVariableBinding.EtlSessionHour2:
                        return this.StartDateTime.Hour.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionMinute:
                        return this.StartDateTime.Minute.ToString();

                    case EtlVariableBinding.EtlSessionMinute2:
                        return this.StartDateTime.Minute.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionSecond:
                        return this.StartDateTime.Second.ToString();

                    case EtlVariableBinding.EtlSessionSecond2:
                        return this.StartDateTime.Second.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionUtcDate:
                        return GetISODate(this.StartUtcDateTime);

                    case EtlVariableBinding.EtlSessionUtcDateTime:
                        return GetISODateTime(this.StartUtcDateTime);

                    case EtlVariableBinding.EtlSessionUtcYear:
                        return this.StartUtcDateTime.Year.ToString();

                    case EtlVariableBinding.EtlSessionUtcYear4:
                        return this.StartUtcDateTime.Year.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionUtcMonth:
                        return this.StartUtcDateTime.Month.ToString();

                    case EtlVariableBinding.EtlSessionUtcMonth2:
                        return this.StartUtcDateTime.Month.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionUtcDay:
                        return this.StartUtcDateTime.Day.ToString();

                    case EtlVariableBinding.EtlSessionUtcDay2:
                        return this.StartUtcDateTime.Day.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionUtcHour:
                        return this.StartUtcDateTime.Hour.ToString();

                    case EtlVariableBinding.EtlSessionUtcHour2:
                        return this.StartUtcDateTime.Hour.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionUtcMinute:
                        return this.StartUtcDateTime.Minute.ToString();

                    case EtlVariableBinding.EtlSessionUtcMinute2:
                        return this.StartUtcDateTime.Minute.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.EtlSessionUtcSecond:
                        return this.StartUtcDateTime.Second.ToString();

                    case EtlVariableBinding.EtlSessionUtcSecond2:
                        return this.StartUtcDateTime.Second.ToString().PadLeft(2, '0');

                    case EtlVariableBinding.TAB:
                        return "\t";

                    case EtlVariableBinding.CR:
                        return "\r";

                    case EtlVariableBinding.LF:
                        return "\n";

                    case EtlVariableBinding.EmptyString:
                        return "";

                    default:
                        throw new InvalidOperationException(string.Format(Properties.Resources.UnknownVariableBinding, variableInfo.Name, variableInfo.Binding));
                }
            }

            private static string GetISODate(DateTime dt)
            {
                return dt.ToString("yyyy-MM-dd");
            }

            private static string GetISODateTime(DateTime dt)
            {
                //truncate milliseconds and returns datetime in ISO 8601
                return dt.ToString("s");
            }

            #endregion
        }
    }
}

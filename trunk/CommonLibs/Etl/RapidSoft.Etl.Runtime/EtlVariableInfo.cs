using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Xml;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    [DebuggerDisplay("{Name}, {Modifier}, {DefaultValue}, {Binding}")]
    public class EtlVariableInfo : ICloneable
    {
        #region Constructors

        public EtlVariableInfo()
        { 
        }

        public EtlVariableInfo(string name, EtlVariableModifier modifier)
        {
            this.Name = name;
            this.Modifier = modifier;
        }

        public EtlVariableInfo(string name, EtlVariableModifier modifier, string defaultValue)
        {
            this.Name = name;
            this.Modifier = modifier;
            this.DefaultValue = defaultValue;
        }

        public EtlVariableInfo(string name, EtlVariableModifier modifier, EtlVariableBinding binding)
        {
            this.Name = name;
            this.Modifier = modifier;
            this.Binding = binding;
        }

        protected EtlVariableInfo(EtlVariableInfo obj)
        {
            this.Name = obj.Name;
            this.Modifier = obj.Modifier;
            this.DefaultValue = obj.DefaultValue;
            this.Binding = obj.Binding;
            this.IsSecure = obj.IsSecure;
        }

        #endregion

        #region Properties

        [DisplayName("(Name)")]
        public string Name
        {
            get;
            set;
        }

        public EtlVariableModifier Modifier
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

        public EtlVariableBinding Binding
        {
            get;
            set;
        }

        public bool IsSecure
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public object Clone()
        {
            return new EtlVariableInfo(this);
        }

        public override string ToString()
        {
            return this.DefaultValue;
        }

        #endregion
    }
}

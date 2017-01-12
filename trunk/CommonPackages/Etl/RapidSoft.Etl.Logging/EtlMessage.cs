using System;
using System.ComponentModel;
using System.Diagnostics;

namespace RapidSoft.Etl.Logging
{
	[Serializable]
    [DebuggerDisplay("{Text}")]
	public class EtlMessage
	{
        [Category("1. Header")]
        [DisplayName("(EtlPackageId)")]
        public string EtlPackageId
        {
            get;
            set;
        }

        [Category("1. Header")]
        [DisplayName("(EtlSessionId)")]
        public string EtlSessionId
        {
            get;
            set;
        }

        [Category("1. Header")]
        public long SequentialId
        {
            get;
            set;
        }

        //[Category("1. Header")]
        //public string EtlStepId
        //{
        //    get;
        //    set;
        //}

        [Category("1. Header")]
        public string EtlStepName
        {
            get;
            set;
        }

        [Category("1. Header")]
        public EtlMessageType MessageType
        {
            get;
            set;
        }

        [Category("2. Data")]
        public string Text
        {
            get;
            set;
        }

        [Category("2. Data")]
        public long? Flags
        {
            get;
            set;
        }

        [Category("2. Data")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=\"2.0.0.0\", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=\"2.0.0.0\", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string StackTrace
        {
            get;
            set;
        }

        [Category("3. DateTime")]
        public DateTime LogDateTime
        {
            get;
            set;
        }

        [Category("3. DateTime")]
        public DateTime LogUtcDateTime
        {
            get;
            set;
        }
	}
}
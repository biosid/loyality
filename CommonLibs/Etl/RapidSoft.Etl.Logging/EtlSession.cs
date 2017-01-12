using System;
using System.ComponentModel;

namespace RapidSoft.Etl.Logging
{
	[Serializable]
	public class EtlSession
    {
        #region Properties

        [Category("1. Header")]
        [DisplayName("(EtlPackageId)")]
        public string EtlPackageId
        {
            get;
            set;
        }

        [Category("1. Header")]
        [DisplayName("(EtlPackageName)")]
        public string EtlPackageName
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
        public string ParentEtlSessionId { get; set; }

        [Category("1. Header")]
        public string UserName { get; set; }

        [Category("2. Insert")]
        public DateTime InsertDateTime { get; set; }

        [Category("2. Insert")]
        public DateTime InsertUtcDateTime { get; set; }

        [Category("2. Start")]
		public DateTime? StartDateTime { get; set; }

        [Category("2. Start")]
		public DateTime? StartUtcDateTime { get; set; }

        [Category("3. End")]
        public DateTime? EndDateTime { get; set; }

        [Category("3. End")]
        public DateTime? EndUtcDateTime { get; set; }

        [Category("3. End")]
        public EtlStatus Status { get; set; }

        #endregion
    }
}
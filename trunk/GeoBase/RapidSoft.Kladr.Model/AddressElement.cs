using System;
using System.Collections.Generic;

namespace RapidSoft.Kladr.Model
{
    using System.Runtime.Serialization;

    [Serializable()]
    [DataContract]
    public class AddressElement : ICloneable
    {
        #region Constructors

        public AddressElement()
        {
        }

        protected AddressElement(AddressElement obj)
        {
            Code = obj.Code;
            Level = obj.Level;
            Prefix = obj.Prefix;
            Name = obj.Name;
        }

        #endregion

        #region Properties

        [DataMember]
        public string Code
        {
            get;
            set;
        }

        [DataMember]
        public AddressLevel Level
        {
            get;
            set;
        }

        [DataMember]
        public string Prefix
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public bool IsEmpty()
        {
            return
                string.IsNullOrEmpty(Code) &&
                Level <= 0 &&
                string.IsNullOrEmpty(Name) &&
                string.IsNullOrEmpty(Prefix)
            ;
        }

        public bool IsWellFormed()
        {
            return
                (!string.IsNullOrEmpty(Code)) &&
                (Level > 0) &&
                (!string.IsNullOrEmpty(Name)) &&
                (!string.IsNullOrEmpty(Prefix))
            ;
        }

        public virtual object Clone()
        {
            return new AddressElement(this);
        }

        #endregion
    }
}

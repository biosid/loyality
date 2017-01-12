using System;
using System.Collections.Generic;

namespace RapidSoft.Kladr.Model
{
    [Serializable()]
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

        public string Code
        {
            get;
            set;
        }

        public AddressLevel Level
        {
            get;
            set;
        }

        public string Prefix
        {
            get;
            set;
        }

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

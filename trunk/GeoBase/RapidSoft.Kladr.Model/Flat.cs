using System;
using System.Collections.Generic;

namespace RapidSoft.Kladr.Model
{
    [Serializable()]
    public class Flat : AddressElement
    {
        #region Constructors

        public Flat()
    	{
			this.Level = AddressLevel.Flat;
    	}

        protected Flat(Flat obj)
            : base(obj)
        {
            this.FlatNumber = obj.FlatNumber;
            this.HouseEntrance = obj.HouseEntrance;
            this.Floor = obj.Floor;
        }

        #endregion

        #region Properties

        public string FlatNumber
        {
            get;
            set;
        }

        public int? HouseEntrance
        {
            get;
            set;
        }

        public int? Floor
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override object Clone()
        {
            return new Flat(this);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace RapidSoft.Kladr.Model
{
    [Serializable()]
    public class House : AddressElement
    {
        #region Constructors

        public House()
    	{
			this.Level = AddressLevel.House;
    	}

        protected House(House obj)
            : base(obj)
        {
            this.HouseNumberBase = obj.HouseNumberBase;
            this.HouseBlock = obj.HouseBlock;
            this.HouseBuilding = obj.HouseBuilding;
            this.HouseFraction = obj.HouseFraction;
            this.IsEstate = obj.IsEstate;
        }

        #endregion

        #region Properties

        public string HouseNumberBase
        {
            get;
            set;
        }

        public string HouseBlock
        {
            get;
            set;
        }

        public string HouseBuilding
        {
            get;
            set;
        }

        public string HouseFraction
        {
            get;
            set;
        }

        public bool IsEstate
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override object Clone()
        {
            return new House(this);
        }

        #endregion
    }
}

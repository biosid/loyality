using System;
using System.Collections.Generic;

namespace RapidSoft.Kladr.Model
{
    [Serializable()]
    public class KladrAddress : ICloneable
    {
        private AddressElement _region;
        private AddressElement _district;
        private AddressElement _city;
        private AddressElement _town;
        private AddressElement _street;
        private House _house;
        private Flat _flat;

        #region Constructors

        public KladrAddress()
        {
        }

        protected KladrAddress(KladrAddress obj)
        {
            this.AddressLevel = obj.AddressLevel;
            this.Region = (AddressElement)(obj.Region != null ? obj.Region.Clone() : null);
            this.District = (AddressElement)(obj.District != null ? obj.District.Clone() : null);
            this.City = (AddressElement)(obj.City != null ? obj.City.Clone() : null);
            this.Street = (AddressElement)(obj.Street != null ? obj.Street.Clone() : null);
            this.Town = (AddressElement)(obj.Town != null ? obj.Town.Clone() : null);
            this.House = (House)(obj.House != null ? obj.House.Clone() : null);
            this.Flat = (Flat)(obj.Flat != null ? obj.Flat.Clone() : null);

            this.FullText = obj.FullText;
        }

        #endregion

        #region Properties

        public AddressLevel AddressLevel { get; set; }

        public AddressElement Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
                if (_region != null)
                {
                    _region.Level = AddressLevel.Region;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public AddressElement District
        {
            get
            {
                return _district;
            }
            set
            {
                _district = value;
                if (_district != null)
                {
                    _district.Level = AddressLevel.District;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public AddressElement City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                if (_city != null)
                {
                    _city.Level = AddressLevel.City;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public AddressElement Town
        {
            get
            {
                return _town;
            }
            set
            {
                _town = value;
                if (_town != null)
                {
                    _town.Level = AddressLevel.Town;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public AddressElement Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
                if (_street != null)
                {
                    _street.Level = AddressLevel.Street;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public House House
        {
            get
            {
                return _house;
            }
            set
            {
                _house = value;
                if (_house != null)
                {
                    _house.Level = AddressLevel.House;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public Flat Flat
        {
            get
            {
                return _flat;
            }
            set
            {
                _flat = value;
                if (_flat != null)
                {
                    _flat.Level = AddressLevel.Flat;
                }
                AddressLevel = this.GetAddressLevel();
            }
        }

        public string FullText { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Последний элемент с кодом с уровнем равным или меньшим заданного.
        /// </summary>
        public AddressElement GetLastElementWithCode(AddressLevel level)
        {
            for (var i = (int)level; i > 0; i--)
            {
                var element = GetElement((AddressLevel)i);

                if (element != null && !String.IsNullOrEmpty(element.Code))
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Элемент с указанным уровнем.
        /// </summary>
        public AddressElement GetElement(AddressLevel level)
        {
            switch (level)
            {
                case AddressLevel.Region:
                    return Region;

                case AddressLevel.District:
                    return District;

                case AddressLevel.City:
                    return City;

                case AddressLevel.Town:
                    return Town;

                case AddressLevel.Street:
                    return Street;

                case AddressLevel.House:
                    return House;

                case AddressLevel.Flat:
                    return Flat;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Все элементы адреса.
        /// </summary>
        public IEnumerable<AddressElement> GetNotNullElements()
        {
            return GetNotNullElements(AddressLevel.House);
        }

        /// <summary>
        /// Все элементы адреса.
        /// </summary>
        public IEnumerable<AddressElement> GetElementsWithCode()
        {
            return GetElementsWithCode(AddressLevel.House);
        }

        /// <summary>
        /// Элементы с кодом с уровнем равным или меньшим заданного.
        /// </summary>
        public IEnumerable<AddressElement> GetElementsWithCode(AddressLevel maxAccuracyLevel)
        {
            if (Region != null && (int)maxAccuracyLevel >= (int)AddressLevel.Region && !string.IsNullOrEmpty(Region.Code))
            {
                yield return Region;
            }

            if (District != null && (int)maxAccuracyLevel >= (int)AddressLevel.District && !string.IsNullOrEmpty(District.Code))
            {
                yield return District;
            }

            if (City != null && (int)maxAccuracyLevel >= (int)AddressLevel.City && !string.IsNullOrEmpty(City.Code))
            {
                yield return City;
            }

            if (Town != null && (int)maxAccuracyLevel >= (int)AddressLevel.Town && !string.IsNullOrEmpty(Town.Code))
            {
                yield return Town;
            }

            if (Street != null && (int)maxAccuracyLevel >= (int)AddressLevel.Street && !string.IsNullOrEmpty(Street.Code))
            {
                yield return Street;
            }

            if (House != null && (int)maxAccuracyLevel >= (int)AddressLevel.House && !string.IsNullOrEmpty(House.Code))
            {
                yield return House;
            }
        }

        /// <summary>
        /// Элементы с кодом с уровнем равным или меньшим заданного.
        /// </summary>
        public IEnumerable<AddressElement> GetNotNullElements(AddressLevel maxAccuracyLevel)
        {
            if (Region != null && (int)maxAccuracyLevel >= (int)AddressLevel.Region)
            {
                yield return Region;
            }

            if (District != null && (int)maxAccuracyLevel >= (int)AddressLevel.District)
            {
                yield return District;
            }

            if (City != null && (int)maxAccuracyLevel >= (int)AddressLevel.City)
            {
                yield return City;
            }

            if (Town != null && (int)maxAccuracyLevel >= (int)AddressLevel.Town)
            {
                yield return Town;
            }

            if (Street != null && (int)maxAccuracyLevel >= (int)AddressLevel.Street)
            {
                yield return Street;
            }

            if (House != null && (int)maxAccuracyLevel >= (int)AddressLevel.House)
            {
                yield return House;
            }

            if (Flat != null && (int)maxAccuracyLevel >= (int)AddressLevel.Flat)
            {
                yield return Flat;
            }
        }



        public bool IsWellFormed()
        {
            foreach (var elem in GetNotNullElements())
            {
                if (elem.IsWellFormed())
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return FullText ?? "";
        }

        public virtual object Clone()
        {
            return new KladrAddress(this);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public class EtlFieldValidationRule
    {
        #region Constructors

        public EtlFieldValidationRule()
        {
        }

        #endregion

        #region Constants

        private const char NEGATIVE_CHAR = '-';

        #endregion

        #region Properties

        public string SourceName
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public bool CanBeNull
        {
            get;
            set;
        }

        public bool CanBeEmpty
        {
            get;
            set;
        }

        public int MinLength
        {
            get;
            set;
        }

        public int? MaxLength
        {
            get;
            set;
        }

        public string Regex
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public virtual bool IsValid(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return this.CanBeNull;
            }
            else
            {
                return IsValid(value.ToString());
            }
        }

        public virtual bool IsValid(string value)
        {
            if (value == null)
            {
                return this.CanBeNull;
            }

            if (value == "")
            {
                return this.CanBeEmpty;
            }

            if (!ValidateLength(value))
            {
                return false;
            }

            if (!ValidatePattern(value))
            {
                return false;
            }

            return true;
        }

        private bool ValidateLength(string value)
        {
            if (value.Length < this.MinLength)
            {
                return false;
            }

            if (this.MaxLength.HasValue)
            {
                if (value.Length > this.MaxLength.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidatePattern(string value)
        {
            if (string.IsNullOrEmpty(this.Regex))
            {
                return true;
            }

            var regex = new Regex(this.Regex);
            if (!regex.IsMatch(value))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}

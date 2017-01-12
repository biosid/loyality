using System;

namespace Rapidsoft.VTB24.Reports.Site.Models.Shared
{
    public class DateModel
    {
        public DateModel() { }

        public DateModel(DateTime value)
        {
            Value = value;
        }

        private DateTime _value;

        public DateTime Value
        {
            get { return _value; }
            set { _value = value.Date; }
        }

        public override string ToString()
        {
            return _value.ToString("dd.MM.yyyy");
        }
    }
}

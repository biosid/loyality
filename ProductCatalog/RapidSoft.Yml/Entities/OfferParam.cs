namespace RapidSoft.YML.Entities
{
    public class OfferParam
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public string Value { get; set; }

        #region Equality members

        public bool Equals(OfferParam other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Unit, Unit) && Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (OfferParam)) return false;
            return Equals((OfferParam) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Name != null ? Name.GetHashCode() : 0);
                result = (result*397) ^ (Unit != null ? Unit.GetHashCode() : 0);
                result = (result*397) ^ (Value != null ? Value.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}
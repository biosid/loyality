namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public class ServiceLogProperty
    {
        public string Name { get; private set; }

        public object Value { get; private set; }

        public ServiceLogProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}

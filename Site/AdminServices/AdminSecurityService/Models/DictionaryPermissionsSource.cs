using System.Collections.Generic;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models
{
    public class DictionaryPermissionsSource : IPermissionsSource
    {
        public string Name { get; private set; }

        public IEnumerable<string> this[PermissionKeys key]
        {
            get
            {
                string[] result;
                return _values.TryGetValue(key, out result) ? result : null;
            }
        }

        public IEnumerable<KeyValuePair<PermissionKeys, string[]>> Enumerate()
        {
            return _values;
        }

        public DictionaryPermissionsSource(string name, Dictionary<PermissionKeys, string[]> values)
        {
            Name = name;
            _values = values;
        }

        public DictionaryPermissionsSource(Dictionary<PermissionKeys, string[]> values)
        {
            _values = values;
        }

        private readonly Dictionary<PermissionKeys, string[]> _values;
    }
}

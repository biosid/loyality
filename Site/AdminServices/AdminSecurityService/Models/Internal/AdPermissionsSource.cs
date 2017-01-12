using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models.Internal
{
    internal class AdPermissionsSource : IPermissionsSource
    {
        public string Name { get; private set; }

        public IEnumerable<string> this[PermissionKeys key]
        {
            get
            {
                var attrName = _attrMap[key];
                return _obj.Properties.Contains(attrName)
                           ? Split(_obj.Properties[attrName].Cast<string>())
                           : null;
            }
        }

        public IEnumerable<KeyValuePair<PermissionKeys, string[]>> Enumerate()
        {
            return _attrMap.Where(item => _obj.Properties.Contains(item.Value))
                           .Select(item => new KeyValuePair<PermissionKeys, string[]>(
                                               item.Key,
                                               Split(_obj.Properties[item.Value].Cast<string>()).ToArray()));
        }

        public AdPermissionsSource(string name, SearchResult obj, Dictionary<PermissionKeys, string> attrMap)
        {
            Name = name;
            _obj = obj;
            _attrMap = attrMap;
        }

        public AdPermissionsSource(SearchResult obj, Dictionary<PermissionKeys, string> attrMap)
        {
            _obj = obj;
            _attrMap = attrMap;
        }

        private readonly SearchResult _obj;
        private readonly Dictionary<PermissionKeys, string> _attrMap;

        private static IEnumerable<string> Split(IEnumerable<string> propertyValues)
        {
            return propertyValues.SelectMany(s => s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                 .Select(s => s.Trim())
                                 .Where(s => !string.IsNullOrEmpty(s));
        }
    }
}

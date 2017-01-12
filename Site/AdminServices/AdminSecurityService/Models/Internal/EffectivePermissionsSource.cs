using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models.Internal
{
    internal class EffectivePermissionsSource : IPermissionsSource
    {
        public string Name { get; private set; }

        public IEnumerable<string> this[PermissionKeys key]
        {
            get
            {
                var values = _sources.Select(source => source[key])
                                     .Where(v => v != null)
                                     .ToArray();
                return values.Length > 0
                           ? values.SelectMany(v => v).Distinct()
                           : null;
            }
        }

        public IEnumerable<KeyValuePair<PermissionKeys, string[]>> Enumerate()
        {
            return _sources.SelectMany(source => source.Enumerate())
                           .GroupBy(item => item.Key)
                           .Select(g => new KeyValuePair<PermissionKeys, string[]>(
                                            g.Key,
                                            g.SelectMany(item => item.Value).Distinct().ToArray()));
        }

        public EffectivePermissionsSource(string name, IPermissionsSource[] sources)
        {
            Name = name;
            _sources = sources;
        }

        public EffectivePermissionsSource(IPermissionsSource[] sources)
        {
            _sources = sources;
        }

        private readonly IPermissionsSource[] _sources;
    }
}

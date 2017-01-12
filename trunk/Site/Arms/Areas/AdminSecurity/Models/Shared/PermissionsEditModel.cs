using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminSecurityService;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminSecurity.Models.Shared
{
    public class PermissionsEditModel
    {
        public const string PERMISSION_NAME_TEMPLATE = "Permission[{0}]";

        public PermissionNode[] Nodes { get; set; }

        public Dictionary<PermissionKeys, ListItemModel[]> Lists { get; set; }

        public IPermissionsSource Permissions { get; set; }

        public IPermissionsSource[] InheritedPermissions { get; set; }

        public static PermissionsEditModel Map(NameValueCollection original, PermissionNode[] nodes, IPermissionsSource[] inheritedPermissions)
        {
            var model = new PermissionsEditModel
            {
                Nodes = nodes,
                InheritedPermissions = inheritedPermissions
            };

            model.LoadPermissions(original);

            return model;
        }

        private void LoadPermissions(NameValueCollection original)
        {
            var values = new Dictionary<PermissionKeys, string[]>();

            foreach (var node in Nodes)
            {
                ExtractPermissions(original, node, values);
            }

            Permissions = new DictionaryPermissionsSource(values);
        }

        private void ExtractPermissions(NameValueCollection original, PermissionNode node, Dictionary<PermissionKeys, string[]> values)
        {
            var extractChildren = false;

            switch (node.Type)
            {
                case PermissionsNodeTypes.Scope:
                    extractChildren = true;
                    break;

                case PermissionsNodeTypes.Checkbox:
                    var isGranted = ExtractValues(original, node.TargetKey).Any();
                    if (isGranted)
                    {
                        values[node.TargetKey] = new string[0];
                    }
                    extractChildren = isGranted ||
                                      (InheritedPermissions != null &&
                                       InheritedPermissions.WhereGranted(node.TargetKey).Any());
                    break;

                case PermissionsNodeTypes.List:
                    var formValues = ExtractValues(original, node.TargetKey).Where(v => !string.IsNullOrEmpty(v))
                                                                            .ToArray();
                    var hasValues = formValues.Length > 0;
                    if (hasValues)
                    {
                        values[node.TargetKey] = formValues.Contains("*") ? new[] { "*" } : formValues.Distinct().ToArray();
                    }
                    extractChildren = hasValues ||
                                      (InheritedPermissions != null &&
                                       InheritedPermissions.WhereAnyValueGranted(node.TargetKey).Any());
                    break;
            }

            if (extractChildren)
            {
                ExtractChildrenPermissions(original, node, values);
            }
        }

        private void ExtractChildrenPermissions(NameValueCollection original, PermissionNode node, Dictionary<PermissionKeys, string[]> values)
        {
            if (node.AdditionalValues != null)
            {
                foreach (var additionalItem in node.AdditionalValues)
                {
                    values[additionalItem.Key] = additionalItem.Value;
                }
            }

            if (node.Children == null)
                return;

            foreach (var child in node.Children)
            {
                ExtractPermissions(original, child, values);
            }
        }

        private static IEnumerable<string> ExtractValues(NameValueCollection original, PermissionKeys key)
        {
            return original.GetValues(string.Format(PERMISSION_NAME_TEMPLATE, key)) ?? Enumerable.Empty<string>();
        }
    }
}

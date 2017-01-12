using System;
using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Helpers
{
    public static class PermissionsSourceExtensions
    {
        public static bool IsGranted(this IPermissionsSource source, PermissionKeys key)
        {
            return source[key] != null;
        }

        public static bool IsAnyGranted(this IPermissionsSource source, params PermissionKeys[] keys)
        {
            return keys.Any(source.IsGranted);
        }

        public static bool IsAllGranted(this IPermissionsSource source, params PermissionKeys[] keys)
        {
            return keys.All(source.IsGranted);
        }

        public static void AssertAllGranted(this IPermissionsSource source, params PermissionKeys[] keys)
        {
            if (!source.IsAllGranted(keys))
                throw new AccessDeniedException(keys);
        }

        public static bool IsWildcardGranted(this IPermissionsSource source, PermissionKeys key)
        {
            var values = source[key];
            return values != null && values.Contains("*");
        }

        public static bool IsExplicitlyGranted(this IPermissionsSource source, PermissionKeys key, string value)
        {
            var values = source[key];
            return values != null && values.Contains(value);
        }

        public static bool IsAnyValueGranted(this IPermissionsSource source, PermissionKeys key)
        {
            var values = source[key];
            return values != null && values.Any();
        }

        public static bool IsGranted<T>(this IPermissionsSource source, PermissionKeys key, T value)
            where T : struct
        {
            var values = source[key];
            if (values == null)
                return false;

            var parser = GetParser<T>();
            if (parser == null)
                throw new NotSupportedException();

            return values.Any(v => v == "*" || parser.Parse(v).Equals(value));
        }

        public static string[] GetGranted(this IPermissionsSource source, PermissionKeys key)
        {
            var permissionValues = source[key];
            if (permissionValues == null)
                return null;

            var permissionValuesList = permissionValues.ToArray();

            if (permissionValuesList.Contains("*"))
                return new string[0];

            return permissionValuesList.Length > 0
                       ? permissionValuesList
                       : null;
        }

        public static T[] GetGranted<T>(this IPermissionsSource source, PermissionKeys key)
            where T : struct
        {
            var parser = GetParser<T>();
            if (parser == null)
                throw new NotSupportedException();

            var permissionValues = source[key];
            if (permissionValues == null)
                return null;

            var permissionValuesList = permissionValues.ToArray();

            if (permissionValuesList.Any(v => v == "*"))
                return new T[0];

            var parsedValues = permissionValuesList.Select(parser.Parse)
                                                   .Where(v => v.HasValue)
                                                   .Select(v => v.Value)
                                                   .ToArray();

            return parsedValues.Length > 0 ? parsedValues : null;
        }

        public static IEnumerable<IPermissionsSource> WhereGranted(this IEnumerable<IPermissionsSource> sources, PermissionKeys key)
        {
            return sources.Where(s => s.IsGranted(key));
        }

        public static IEnumerable<IPermissionsSource> WhereWildcardGranted(this IEnumerable<IPermissionsSource> sources, PermissionKeys key)
        {
            return sources.Where(s => s.IsWildcardGranted(key));
        }

        public static IEnumerable<IPermissionsSource> WhereExplicitlyGranted(this IEnumerable<IPermissionsSource> source, PermissionKeys key, string value)
        {
            return source.Where(s => s.IsExplicitlyGranted(key, value));
        }

        public static IEnumerable<IPermissionsSource> WhereAnyValueGranted(this IEnumerable<IPermissionsSource> sources, PermissionKeys key)
        {
            return sources.Where(s => s.IsAnyValueGranted(key));
        }

        private interface IParser<T> where T : struct
        {
            T? Parse(string value);
        }

        private class IntegerParser : IParser<int>
        {
            public int? Parse(string value)
            {
                int result;
                return int.TryParse(value, out result) ? result : (int?) null;
            }
        }

        static PermissionsSourceExtensions()
        {
            Parsers = new object[] { new IntegerParser() };
        }

        private static readonly object[] Parsers;

        private static IParser<T> GetParser<T>() where T : struct
        {
            return Parsers.OfType<IParser<T>>().FirstOrDefault();
        }
    }
}

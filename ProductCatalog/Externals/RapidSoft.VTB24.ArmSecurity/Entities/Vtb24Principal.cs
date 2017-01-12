namespace RapidSoft.VTB24.ArmSecurity.Entities
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;
    using System.Security.Principal;

    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    public class Vtb24Principal : IVtb24Principal
    {
        private readonly UserIdentity identity;

        private readonly IEnumerable<SearchResultWrapper> searchResultWrappers;

        public Vtb24Principal(UserIdentity identity, IEnumerable<SearchResult> userGroups)
        {
            this.identity = identity;
            this.searchResultWrappers = userGroups == null ? null : userGroups.Select(x => new SearchResultWrapper(x));
        }

        public IIdentity Identity
        {
            get
            {
                return this.identity;
            }
        }

        public bool HasPermission(string permission)
        {
            if (!this.identity.IsAuthenticated)
            {
                return false;
            }

            if (this.searchResultWrappers.Any(x => x.Contains(permission)))
            {
                return true;
            }

            return false;
        }

        public bool HasPermissions(IEnumerable<string> permissions)
        {
            return permissions.All(this.HasPermission);
        }

        public bool HasPermissionForPartner(string permission, string partnerId)
        {
            if (string.IsNullOrWhiteSpace(partnerId))
            {
                return false;
            }

            if (!this.identity.IsAuthenticated)
            {
                return false;
            }

            // NOTE: Ниже для случая, если объединяем.
            var hasPermission = this.searchResultWrappers.Any(x => x.Contains(permission));
            var forPartner = this.searchResultWrappers.Any(x => x.HasPartnerId(partnerId));
            var has = hasPermission && forPartner;

            // NOTE: Ниже для случая, если не объединяем.
            // var has = this.searchResultWrappers.Where(x => x.Contains(permission))
            //        .Any(userGroup => userGroup.HasPartnerId(partnerId));

            return has;
        }

        public bool HasPermissionsForPartner(IEnumerable<string> permissions, string partnerId)
        {
            return permissions.All(x => this.HasPermissionForPartner(x, partnerId));
        }

        internal class SearchResultWrapper
        {
            internal const string VtbPermissionPrefix = "vtb";
            internal const string AllObjectsSymbol = "*";
            internal static readonly string PartnerIdsAttrName = "vtbPartnerIds";
            private static readonly char[] PartnerIdsSeparators = new[] { ',' };

            private readonly SearchResult searchResult;
            private IList<string> partnerIds;

            public SearchResultWrapper(SearchResult searchResult)
            {
                this.searchResult = searchResult;
            }

            public bool Contains(string propertyName)
            {
                var lower = VtbPermissionPrefix + propertyName.ToLower();
                return this.searchResult.Properties.Contains(lower);
            }

            public bool HasPartnerId(string partnerId)
            {
                if (this.partnerIds == null)
                {
                    var value = this.searchResult.GetValues<string>(PartnerIdsAttrName);
                    this.partnerIds =
                        value.SelectMany(x => x.Split(PartnerIdsSeparators, StringSplitOptions.RemoveEmptyEntries))
                             .Select(x => x.Trim())
                             .ToList();
                }

                return this.partnerIds.Contains(AllObjectsSymbol) || this.partnerIds.Contains(partnerId);
            }
        }
    }
}
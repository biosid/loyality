using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Exceptions;
using Vtb24.Site.Content.Pages.Models.Inputs;
using Vtb24.Site.Content.Pages.Models.Outputs;

namespace Vtb24.Site.Content.Pages
{
    public class AdminPagesManagement : IAdminPagesManagement
    {
        public AdminPagesManagement(IAdminSecurityService security)
        {
            _security = security;
            _pages = new PagesManagement();
        }

        private readonly IAdminSecurityService _security;
        private readonly IPagesManagement _pages;

        public GetAllPagesResult GetAllPlainPages(GetPlainPagesOptions options, PagingSettings paging)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.GetAllPlainPages(options, paging);
        }

        public Page GetPlainPageById(Guid id, GetPlainPagesOptions options)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.GetPlainPageById(id, options);
        }

        public bool HasPageWithUrl(string url, GetPlainPagesOptions options = new GetPlainPagesOptions())
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.HasPageWithUrl(url, options);
        }

        public Page GetPageByUrl(string url, GetPlainPagesOptions options)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.GetPageByUrl(url, options);
        }

        public PageData GetPageVersionById(Guid id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.GetPageVersionById(id);
        }

        public void ReloadBuiltinPagesFromConfiguration()
        {
            throw new AccessDeniedException();
        }

        public Page GetOfferPageByPartnerId(int partnerId, bool loadFullHistory)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.GetOfferPageByPartnerId(partnerId, loadFullHistory);
        }

        public bool IsPartnerGotOfferPage(int partnerId)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            return _pages.IsPartnerGotOfferPage(partnerId);
        }

        public Guid CreatePlainPage(CreatePlainPageOptions options)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var page = context
                    .Pages
                    .AsNoTracking()
                    .FirstOrDefault(p => p.History.CurrentVersion.Data.Url == options.Data.Url);

                if (page != null)
                    throw new PageUrlExistsException(page.Id);

                var snapshot = new PageSnapshot
                {
                    Author = _security.CurrentUser,
                    When = DateTime.Now,
                    Data = options.Data
                };

                try
                {
                    page = Page.Create(snapshot, context);
                }
                catch (DbEntityValidationException)
                {
                    throw new PageInvalidException();
                }

                page.IsBuiltin = false;
                page.Status = PageStatus.NotActive;
                page.Type = PageType.Plain;

                context.Pages.Add(page);

                context.SaveChanges();

                return page.Id;
            }
        }

        public void UpdatePlainPage(UpdatePlainPageOptions options)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var otherPage = context
                    .Pages
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id != options.Id && p.History.CurrentVersion.Data.Url == options.Data.Url);

                if (otherPage != null)
                    throw new PageUrlExistsException(otherPage.Id);

                var snapshot = new PageSnapshot
                {
                    Author = _security.CurrentUser,
                    When = DateTime.Now,
                    Data = options.Data
                };

                try
                {
                    Page.Update(options.Id, snapshot, context);
                }
                catch (ObjectNotFoundException)
                {
                    throw new PageNotFoundException();
                }
                catch (DbEntityValidationException)
                {
                    throw new PageInvalidException();
                }
            }
        }

        public void RemovePlainPage(Guid id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var page = context.Pages.Find(id);
                if (page == null)
                    throw new PageNotFoundException();
                if (page.IsBuiltin)
                    throw new PageRemovalFailedException();

                context.Pages.Remove(page);

                context.SaveChanges();
            }
        }

        public void ChangePlainPagesStatus(Guid[] ids, PageStatus status)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                foreach (var page in ids.Select(id => context.Pages
                                                             .Find(id)
                                                             .IncludeHistory(context))
                                        .Where(p => p != null && !p.IsBuiltin)
                                        .ToArray())
                    page.Status = status;

                context.SaveChanges();
            }
        }

        public GetAllPagesResult GetAllOfferPages(PagingSettings paging)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var query = context.Pages
                                   .AsNoTracking()
                                   .Where(p => p.Type == PageType.Offer)
                                   .Include(p => p.History.CurrentVersion)
                                   .OrderBy(p => p.History.CurrentVersion.Data.Title);

                var pages = query.Skip(paging.Skip)
                                 .Take(paging.Take)
                                 .ToArray();

                var totalCount = query.Count();

                return new GetAllPagesResult(pages, totalCount, paging);
            }
        }

        public int CreateOfferPage(CreateOfferPageOptions options)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var page = context
                            .Pages
                            .AsNoTracking()
                            .FirstOrDefault(p => p.History.CurrentVersion.Data.Url == options.Data.Url);

                if (page != null)
                {
                    throw new PageUrlExistsException(page.Id);
                }

                var partnerId = options.PartnerId.ToString(CultureInfo.InvariantCulture);
                var offer = context
                                .Pages
                                .AsNoTracking()
                                .FirstOrDefault(p => p.ExternalId == partnerId && p.Type == PageType.Offer);

                if (offer != null)
                {
                    throw new OfferPageExistsException(options.PartnerId);
                }

                var snapshot = new PageSnapshot
                {
                    Author = _security.CurrentUser,
                    When = DateTime.Now,
                    Data = options.Data
                };

                try
                {
                    page = Page.Create(snapshot, context);
                }
                catch (DbEntityValidationException)
                {
                    throw new PageInvalidException();
                }

                page.ExternalId = partnerId;
                page.IsBuiltin = false;
                page.Status = PageStatus.NotActive;
                page.Type = PageType.Offer;

                context.Pages.Add(page);

                context.SaveChanges();

                return options.PartnerId;
            }
        }

        public void UpdateOfferPage(UpdateOfferPageOptions options)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var partnerIdStr = options.PartnerId.ToString(CultureInfo.InvariantCulture);

                var otherPage = context
                    .Pages
                    .AsNoTracking()
                    .FirstOrDefault(p => p.ExternalId != partnerIdStr && p.History.CurrentVersion.Data.Url == options.Data.Url);

                if (otherPage != null)
                    throw new PageUrlExistsException(otherPage.Id);

                var page = context
                                .Pages
                                .AsNoTracking()
                                .FirstOrDefault(p => p.ExternalId == partnerIdStr && p.Type == PageType.Offer);

                if (page == null)
                    throw new PageNotFoundException();

                var snapshot = new PageSnapshot
                {
                    Author = _security.CurrentUser,
                    When = DateTime.Now,
                    Data = options.Data
                };

                try
                {
                    Page.Update(page.Id, snapshot, context);
                }
                catch (ObjectNotFoundException)
                {
                    throw new PageNotFoundException();
                }
                catch (DbEntityValidationException)
                {
                    throw new PageInvalidException();
                }
            }
        }

        public void RemoveOfferPage(int partnerId)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var partnerIdStr = partnerId.ToString(CultureInfo.InvariantCulture);
                
                var page = context
                            .Pages
                            .FirstOrDefault(p => p.ExternalId == partnerIdStr && p.Type == PageType.Offer);
                
                if (page == null)
                    throw new PageNotFoundException();
                
                if (page.IsBuiltin)
                    throw new PageRemovalFailedException();

                context.Pages.Remove(page);

                context.SaveChanges();
            }
        }

        public void ChangeOfferPagesStatus(int[] partnerIds, PageStatus status)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            using (var context = new ContentServiceDbContext())
            {
                var pages = partnerIds.Select(id => id.ToString(CultureInfo.InvariantCulture))
                                        .ToArray()
                                        .Select(id =>
                                        {
                                            var page = context
                                                        .Pages
                                                        .FirstOrDefault(p => p.ExternalId == id && 
                                                                        p.Type == PageType.Offer);
                                            return page != null ? page.IncludeHistory(context) : null;
                                        })
                                        .Where(p => p != null)
                                        .ToArray();

                foreach (var page in pages)
                {
                    page.Status = status;
                }

                context.SaveChanges();
            }
        }
    }
}

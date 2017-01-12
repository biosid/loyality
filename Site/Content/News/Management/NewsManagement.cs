using System;
using System.Linq;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Models.Exceptions;
using Vtb24.Site.Content.News.Management.Models.Inputs;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.News.Models.Exceptions;
using Vtb24.Site.Content.News.Models.Outputs;
using Vtb24.Site.Content.Snapshots;
using Vtb24.Site.Content.Snapshots.Models;

namespace Vtb24.Site.Content.News.Management
{
    /// <summary>
    /// Сервис для работы с новостями
    /// </summary>
    public class NewsManagement : INewsManagement
    {
        public NewsManagement(IAdminSecurityService security)
        {
            _snapshots = new Snapshots.Snapshots();
            _security = security;
        }

        private readonly ISnapshots _snapshots;
        private readonly IAdminSecurityService _security;

        public NewsMessage GetById(long id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            using (var context = new ContentServiceDbContext())
            {
                try
                {
                    return context.NewsMessages.Find(id);
                }
                catch (Exception ex)
                {
                    throw new ContentManagementServiceException(ex.Message);
                }
            }
        }

        public void Create(UpdateNewsMessageOption option)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            var author = _security.CurrentUser;

            using (var context = new ContentServiceDbContext())
            {
                try
                {
                    var msg = new NewsMessage
                    {
                        Title = option.Title,
                        Priority = option.Priority,
                        StartDate = option.StartDate,
                        EndDate = option.EndDate,
                        IsPublished = option.IsPublished,
                        Picture = option.Picture,
                        Url = option.Url,
                        Description = option.Description,
                        Author = author,
                        Segment = option.Segment,
                    };
                    var result = context.NewsMessages.Add(msg);

                    context.SaveChanges(); // REVIEW: Нужно, чтобы получить id...

                    _snapshots.Create(result.Id, result, author);
                }
                catch (Exception ex)
                {
                    throw new ContentManagementServiceException(ex.Message);
                }
            }
        }

        public void Edit(long id, UpdateNewsMessageOption option)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            var author = _security.CurrentUser;

            using (var context = new ContentServiceDbContext())
            {
                try
                {
                    var msg = context.NewsMessages.Find(id);
                    if (msg == null)
                    {
                        throw new NewsMessageNotFoundException(id);
                    }                    

                    var newMsg = new NewsMessage
                    {
                        Id = id,
                        Title = option.Title,
                        Priority = option.Priority,
                        Author = author,
                        Url = option.Url,
                        StartDate = option.StartDate,
                        EndDate = option.EndDate,
                        Picture = option.Picture,
                        Segment = option.Segment,
                        Description = option.Description,
                        IsPublished = option.IsPublished
                    };
                    
                    if (_snapshots.Create(id, newMsg, author))
                    {
                        msg.Id = newMsg.Id;
                        msg.Title = newMsg.Title;
                        msg.Priority = newMsg.Priority;
                        msg.Author = newMsg.Author;
                        msg.Url = newMsg.Url;
                        msg.StartDate = newMsg.StartDate;
                        msg.EndDate = newMsg.EndDate;
                        msg.Picture = newMsg.Picture;
                        msg.Segment = newMsg.Segment;
                        msg.Description = newMsg.Description;
                        msg.IsPublished = newMsg.IsPublished;

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new ContentManagementServiceException(ex.Message);
                }
            }
        }

        public void Delete(long id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            using (var context = new ContentServiceDbContext())
            {
                try
                {
                    var msg = context.NewsMessages.Find(id);
                    if (msg == null)
                    {
                        throw new NewsMessageNotFoundException(id);
                    }
                    
                    context.NewsMessages.Remove(msg);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ContentManagementServiceException(ex.Message);
                }
            }
        }

        public void Publish(long[] ids, bool publish)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            using (var context = new ContentServiceDbContext())
            {
                try
                {
                    var newsMessages = context.NewsMessages.Where(m => ids.Contains(m.Id)).ToArray();
                    foreach (var newsMessage in newsMessages)
                    {
                        newsMessage.IsPublished = publish;
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ContentManagementServiceException(ex.Message);
                }
            }
        }

        public Snapshot<NewsMessage> GetFromHistoryBySnapshotId(string id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            try
            {
                return _snapshots.GetById<NewsMessage>(id);
            }
            catch (Exception ex)
            {
                throw new ContentManagementServiceException(ex.Message);
            }
        }

        public Snapshot<NewsMessage>[] GetAllHistoryById(long id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            try
            {
                return _snapshots.GetByEntityId<NewsMessage>(id);
            }
            catch (Exception ex)
            {
                throw new ContentManagementServiceException(ex.Message);
            }
        }

        public GetNewsMessagesResult GetNewsMessages(GetNewsMessagesFilter filter, PagingSettings paging)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            using (var context = new ContentServiceDbContext())
            {
                try
                {
                    var query = context.NewsMessages.Where(m => m.IsPublished || filter.IncludeUnpublished);

                    if (!string.IsNullOrEmpty(filter.Keyword))
                    {
                        query = query
                                .Where(m => m.Title.Contains(filter.Keyword) ||
                                            m.Description.Contains(filter.Keyword))
                                .Select(m => new
                                {
                                    m,
                                    range = m.Title.Contains(filter.Keyword) ? 1 :
                                            m.Description.Contains(filter.Keyword) ? 2 : 0
                                })
                                .OrderBy(m => m.range)
                                .Select(m => m.m);
                    }

                    if (filter.From.HasValue)
                    {
                        query = query.Where(m => (m.StartDate >= filter.From.Value) ||
                                                 (!m.EndDate.HasValue && (m.StartDate <= filter.From.Value)));
                    }

                    if (filter.To.HasValue)
                    {
                        query = query.Where(m => m.StartDate <= filter.To.Value);
                    }

                    query = query.OrderBy(m => m.Priority).ThenByDescending(m => m.Segment).ThenBy(m => m.StartDate);

                    var totalCount = query.Count();

                    query = query.Skip(paging.Skip).Take(paging.Take);

                    var result = query.ToArray();

                    return new GetNewsMessagesResult(result, totalCount, paging);

                }
                catch (Exception ex)
                {
                    throw new ContentManagementServiceException(ex.Message);
                }
            }
        }
    }
}

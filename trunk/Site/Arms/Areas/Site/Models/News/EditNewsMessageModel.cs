using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.Snapshots.Models;

namespace Vtb24.Arms.Site.Models.News
{
    public class EditNewsMessageModel
    {
        public long Id { get; set; }

        public string SnapshotId { get; set; }

        [Required(ErrorMessage = "Необходимо указать приоритет новости")]
        public int Priority { get; set; }

        public string PictureUrl { get; set; }

        public bool IsPublished { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату начала новости")]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Необходимо указать название новости")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина названия (256 символов)")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать описание новости")]
        [StringLength(128, ErrorMessage = "Превышена допустимая длина описания (128 символов)")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Необходимо указать ссылку на текст новости")]
        [StringLength(128, ErrorMessage = "Превышена допустимая длина ссылки (128 символов)")]
        public string Url { get; set; }

        public string Segment { get; set; }

        public NewsModelStatus Status { get; set; }

        public NewsMessageHistoryModel[] History { get; set; }

        public DateTime? CreationDate { get; set; }

        public static EditNewsMessageModel Map(Snapshot<NewsMessage> snapshot, Snapshot<NewsMessage>[] history)
        {
            var model = new EditNewsMessageModel
            {
                Id = snapshot.Entity.Id,
                SnapshotId = snapshot.Id,
                CreationDate = snapshot.CreationDate,
                Description = snapshot.Entity.Description,
                Priority = snapshot.Entity.Priority,
                StartDate = snapshot.Entity.StartDate,
                EndDate = snapshot.Entity.EndDate,
                Title = snapshot.Entity.Title,
                Url = snapshot.Entity.Url,
                Segment = snapshot.Entity.Segment,
                IsPublished = snapshot.Entity.IsPublished,
                PictureUrl = snapshot.Entity.Picture,
                History = history.Select (NewsMessageHistoryModel.Map)
                                 .OrderByDescending (h => h.CreationDate)
                                 .ToArray ()
            };
            if (model.History.Length > 0)
            {
                model.History.First ().IsLastVersion = true;
            }

            return model;
        }

        public static EditNewsMessageModel Map(NewsMessage message, Snapshot<NewsMessage>[] history)
        {
            var model = new EditNewsMessageModel
            {
                Id = message.Id,
                Description = message.Description,
                Priority = message.Priority,
                StartDate = message.StartDate,
                EndDate = message.EndDate,
                Title = message.Title,
                Url = message.Url,
                Segment = message.Segment,
                IsPublished = message.IsPublished,
                PictureUrl = message.Picture,
                History = history.Select(NewsMessageHistoryModel.Map)
                                 .OrderByDescending(h => h.CreationDate)
                                 .ToArray()
            };

            if (model.History.Length > 0)
            {
                model.History.First().IsLastVersion = true;
            }

            return model;
        }
    }
}

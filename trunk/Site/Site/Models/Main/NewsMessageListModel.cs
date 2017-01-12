using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Vtb24.Site.Content.News.Models;

namespace Vtb24.Site.Models.Main
{
    public class NewsMessageListModel
    {
        public DateTime Date { get; set; }

        public string PictureUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long Id { get; set; }

        public string Url { get; set; }

        public static NewsMessageListModel Map(NewsMessage original)
        {
            return new NewsMessageListModel
            {
                Date = original.StartDate,
                Description = original.Description,
                PictureUrl = string.Format("{0}{1}", ConfigurationManager.AppSettings["content_pictures_view_path"], original.Picture),
                Title = original.Title,
                Id = original.Id,
                Url = original.Url
            };
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vtb24.Site.Content.Infrastructure;

namespace Vtb24.Site.Content.Pages.Models
{
    [ComplexType]
    public class PageData
    {
        private string _url;
        
        [Required(ErrorMessage = "Необходимо указать URL")]
        [RegularExpression("[^?]+", ErrorMessage = "URL не должен содержать знаков '?'")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина URL (256 символов)")]
        public string Url 
        { 
            get { return _url; }
            set { _url = UrlHelpers.NormalizeUrl(value); }
        }

        [Required(ErrorMessage = "Необходимо указать заголовок")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина заголовка (256 символов)")]
        public string Title { get; set; }

        [StringLength(1024, ErrorMessage = "Превышена допустимая длина ключевых слов (1024 символа)")]
        public string Keywords { get; set; }

        [StringLength(1024, ErrorMessage = "Превышена допустимая длина описания (1024 символа)")]
        public string Description { get; set; }

        public PageLayoutType Layout { get; set; }

        public string Content { get; set; }

        public string Script { get; set; }
    }
}

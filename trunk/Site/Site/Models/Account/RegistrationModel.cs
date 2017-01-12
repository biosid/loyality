using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Account
{
    [Bind(Exclude = "ErrorMessage")]
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Укажите фамилию")]
        [StringLength(50, ErrorMessage = "Превышена допустимая длина фамилии (50 символов)")]
        [RegularExpression("^[а-яА-ЯёЁ'-]+$", ErrorMessage = "Фамилия может состоять только из русских букв, дефиса и апострофа")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [StringLength(50, ErrorMessage = "Превышена допустимая длина имени (50 символов)")]
        [RegularExpression("^[а-яА-ЯёЁ'-]([а-яА-ЯёЁ '-]*[а-яА-ЯёЁ'-])?$", ErrorMessage = "Имя может состоять только из русских букв, дефиса, апострофа и пробела, и не может начинаться или оканчиваться пробелом")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Превышена допустимая длина отчества (50 символов)")]
        [RegularExpression("^[а-яА-ЯёЁ'-]([а-яА-ЯёЁ '-]*[а-яА-ЯёЁ'-])?$", ErrorMessage = "Отчество может состоять только из русских букв, дефиса, апострофа и пробела, и не может начинаться или оканчиваться пробелом")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Укажите год рождения")]
        public int? BirthYear { get; set; }

        [Required(ErrorMessage = "Укажите месяц рождения")]
        public int? BirthMonth { get; set; }

        [Required(ErrorMessage = "Укажите день рождения")]
        public int? BirthDate { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Mask("+7 (999) 999-9999", Constants.PHONE_REGEX, ErrorMessage = "Введённый номер телефона не может быть использован. Пожалуйста, укажите номер мобильного телефона.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Укажите E-mail")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddressAttribute(ErrorMessage = "Неверный формат E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите код с картинки")]
        [StringLength(10, ErrorMessage = "Превышена допустимая длина кода с картинки (10 символов)")]
        [CaptchaValidator(ErrorMessage = "Изображение и введенный текст не совпадают")]
        public string Captcha { get; set; }

        public bool AgreeToTerms { get; set; }

        public string ErrorMessage { get; set; }

        public bool HasErrorMessage
        {
            get { return !string.IsNullOrWhiteSpace(ErrorMessage); }
        }

    }
}
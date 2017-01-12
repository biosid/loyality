using System.ComponentModel.DataAnnotations;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Buy
{
    public class ContactModel
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(ErrorMessage = "Укажите фамилию")]
        [StringLength(50, ErrorMessage = "Превышена допустимая длина фамилии (50 символов)")]
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Укажите имя")]
        [StringLength(50, ErrorMessage = "Превышена допустимая длина имени (50 символов)")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [StringLength(50, ErrorMessage = "Превышена допустимая длина отчества (50 символов)")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [Required(ErrorMessage = "Укажите номер телефона")]
        [Mask("+7 (999) 999-9999", ErrorMessage = "Номер телефона должен быть в формате +7 (xxx) xxx-xxxx")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddress(ErrorMessage = "Неверный формат E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// Email (если для партнера настроена обязательность Email при оформлении заказа)
        /// </summary>
        [Required(ErrorMessage = "Укажите E-mail")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddress(ErrorMessage = "Неверный формат E-mail")]
        public string RequiredEmail { get; set; }

        /// <summary>
        /// Повторный ввод Email (если для партнера настроена обязательность Email при оформлении заказа)
        /// </summary>
        [Required(ErrorMessage = "Повторно введите E-mail")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddress(ErrorMessage = "Неверный формат E-mail")]
        [Compare("RequiredEmail", ErrorMessage = "Введенные адреса E-mail не совпадают")]
        public string ConfirmRequiredEmail { get; set; }

        /// <summary>
        /// Сохранить Email в профиле
        /// </summary>
        public bool SaveEmail { get; set; }
    }
}